using KazusaHSR.GameServer.Resource;
using KazusaHSR.Resource;
using Newtonsoft.Json;

namespace KazusaHSR.GameServer;

public sealed partial class TaskExecutorManager : ITaskExecutorRuntime
{
	private readonly Scene _scene;
	private readonly Session _session;
	private readonly Logger _log;

	private TaskExecutorState _state = TaskExecutorState.Idle;

	private readonly List<PendingWaitPredicate> _pendingPredicates = new();
	private readonly List<PendingWaitCustomString> _pendingWaitCustomStrings = new();
	private readonly List<PendingBeHit> _pendingBeHits = new();

	private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
	{
		Converters = new List<JsonConverter> { new TaskConfigJsonConverter() }
	};

	private enum RuntimeKind
	{
		Scene = 0,
		Adventure = 1,
	}

	private sealed class TaskRuntime
	{
		public RuntimeKind Kind { get; init; }
		public uint GroupId { get; init; }
		public uint OwnerPropEntityId { get; init; }
		public AdventureAbilityContext? AdventureContext { get; init; }
	}

	private sealed class PendingWaitPredicate
	{
		public WaitPredicateSucc WaitConfig { get; init; } = null!;
		public List<TaskConfig> NextTasks { get; init; } = new();
		public TaskRuntime Runtime { get; init; } = null!;
	}

	private sealed class PendingWaitCustomString
	{
		public string CustomString { get; init; } = string.Empty;
		public bool WaitOwnerOnly { get; init; }
		public List<TaskConfig> NextTasks { get; init; } = new();
		public TaskRuntime Runtime { get; init; } = null!;
	}

	private sealed class PendingBeHit
	{
		public uint GroupId { get; init; }
		public uint PropInstId { get; init; }
		public List<TaskConfig> OnBeHitTasks { get; init; } = new();
	}

	public TaskExecutorManager(Scene scene)
	{
		_scene = scene ?? throw new ArgumentNullException(nameof(scene));
		_session = scene.session ?? throw new ArgumentNullException(nameof(scene.session));
		_log = _session.c;
	}

	public TaskExecutorState GetTaskState()
	{
		return _state;
	}

	public void OnTaskBegin()
	{
		ThrowIfDisposed();
		_state = TaskExecutorState.Running;
	}

	public void OnTaskReset()
	{
		ThrowIfDisposed();
		_pendingPredicates.Clear();
		_pendingWaitCustomStrings.Clear();
		_pendingBeHits.Clear();
		_state = TaskExecutorState.Idle;
	}

	public void Dispose()
	{
		if (_state == TaskExecutorState.Disposed)
			return;

		_pendingPredicates.Clear();
		_pendingWaitCustomStrings.Clear();
		_pendingBeHits.Clear();
		_state = TaskExecutorState.Disposed;
	}

	public void StartForGroup(uint groupId, LevelGroupInfo groupInfo)
	{
		ThrowIfDisposed();
		if (groupInfo == null || string.IsNullOrEmpty(groupInfo.LevelGraph))
			return;

		OnTaskBegin();
		StartLevelGraph(groupInfo.LevelGraph, groupId, 0, $"group {groupInfo.GroupGUID}");
	}

	public void StartInitGraphForProp(PropEntity prop)
	{
		ThrowIfDisposed();
		if (prop == null)
			return;

		string path = prop.DbInfo.InitLevelGraph;
		if (string.IsNullOrEmpty(path))
			return;

		OnTaskBegin();
		StartLevelGraph(path, prop.GroupId, prop._EntityId, $"prop GroupId={prop.GroupId}, InstId={prop.DbInfo.ID}");
	}

	public void ExecuteAdventureTasks(AdventureAbilityContext ctx, IEnumerable<TaskConfig> tasks)
	{
		ThrowIfDisposed();
		if (ctx == null || tasks == null)
			return;

		OnTaskBegin();
		ExecuteTaskList(tasks.Where(t => t != null).ToList(), new TaskRuntime
		{
			Kind = RuntimeKind.Adventure,
			GroupId = 0,
			OwnerPropEntityId = 0,
			AdventureContext = ctx,
		});
	}

	public void CreateDummyFight(AdventureAbilityContext ctx)
	{
		ThrowIfDisposed();
		if (ctx == null)
			return;

		ExecuteAdventureTasks(ctx, new TaskConfig[]
		{
			new AdventureTriggerAttack
			{
				OnAttack = [],
			}
		});
	}

	public void OnWaitCustomStringReceived(string customString, uint propEntityId, uint subMissionId)
	{
		if (_pendingWaitCustomStrings.Count == 0)
			return;

		var matches = _pendingWaitCustomStrings
			.Where(p => p.CustomString == customString &&
				(!p.WaitOwnerOnly || (propEntityId != 0 && p.Runtime.OwnerPropEntityId == propEntityId)))
			.ToList();
		if (matches.Count == 0)
			return;

		foreach (var pending in matches)
		{
			_pendingWaitCustomStrings.Remove(pending);
			if (pending.NextTasks.Count > 0)
			{
				ExecuteTaskList(pending.NextTasks, pending.Runtime);
			}

			_log.Message($"[TaskExecutorManager] WaitCustomString satisfied for CustomString='{pending.CustomString}', GroupId={pending.Runtime.GroupId}, OwnerEntityId={pending.Runtime.OwnerPropEntityId}, PropEntityId={propEntityId}, SubMissionId={subMissionId}");
		}
	}

	public void OnPropBeHit(PropEntity prop)
	{
		if (prop == null || _pendingBeHits.Count == 0)
			return;

		uint groupId = prop.GroupId;
		uint instId = prop.DbInfo.ID;

		// EventID
		LevelPropInfo levelPropInfo = prop.DbInfo;
		if (levelPropInfo.EventID != 0)
		{
			_scene.TriggerEvent(levelPropInfo.EventID, prop._EntityId);
		}

		var matches = _pendingBeHits
			.Where(p => p.GroupId == groupId && p.PropInstId == instId)
			.ToList();
		if (matches.Count == 0)
			return;

		foreach (var pending in matches)
		{
			if (pending.OnBeHitTasks.Count == 0)
				continue;

			ExecuteTaskList(pending.OnBeHitTasks, new TaskRuntime
			{
				Kind = RuntimeKind.Scene,
				GroupId = groupId,
				OwnerPropEntityId = prop._EntityId,
				AdventureContext = null,
			});
		}

		_log.Message($"[TaskExecutorManager] OnPropBeHit triggered for GroupId={groupId}, InstId={instId}, Count={matches.Count}");
	}

	public void OnMonstersChanged()
	{
		if (_pendingPredicates.Count == 0)
			return;

		var snapshot = _pendingPredicates.ToList();
		foreach (var pending in snapshot)
		{
			var cond = pending.WaitConfig.Condition;
			if (!EvaluatePredicate(cond, pending.Runtime))
				continue;

			_log.Message($"[TaskExecutorManager] WaitPredicateSucc satisfied for GroupId={pending.Runtime.GroupId} with condition {cond?.GetType().FullName}");
			_pendingPredicates.Remove(pending);

			if (pending.NextTasks.Count > 0)
			{
				ExecuteTaskList(pending.NextTasks, pending.Runtime);
			}
		}
	}

	private void ThrowIfDisposed()
	{
		if (_state == TaskExecutorState.Disposed)
		{
			throw new ObjectDisposedException(nameof(TaskExecutorManager));
		}
	}
}