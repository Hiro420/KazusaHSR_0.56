using KazusaHSR.GameServer.Resource;
using Newtonsoft.Json;

namespace KazusaHSR.GameServer;

public sealed partial class TaskExecutorManager
{
	private void StartLevelGraph(string levelGraphPath, uint groupId, uint ownerPropEntityId, string context)
	{
		try
		{
			var config = LoadLevelGraph(levelGraphPath);
			if (config == null)
			{
				_log.Alert($"[TaskExecutorManager] Failed to load LevelGraph '{levelGraphPath}' for {context}");
				return;
			}

			_log.Message($"[TaskExecutorManager] Starting LevelGraph '{levelGraphPath}' for {context} (GroupId={groupId}, OwnerEntityId={ownerPropEntityId})");

			var runtime = new TaskRuntime
			{
				Kind = RuntimeKind.Scene,
				GroupId = groupId,
				OwnerPropEntityId = ownerPropEntityId,
				AdventureContext = null,
			};

			ExecuteSequences(config.OnInitSequece ?? Array.Empty<LevelTaskSequence>(), runtime);
			ExecuteSequences(config.OnStartSequece ?? Array.Empty<LevelTaskSequence>(), runtime);
		}
		catch (Exception ex)
		{
			_log.Alert($"[TaskExecutorManager] Exception while loading LevelGraph '{levelGraphPath}' for {context}: {ex.Message}");
		}
	}

	private LevelGraphConfig? LoadLevelGraph(string levelGraphPath)
	{
		try
		{
			string basePath = MainApp.resourceManager.loader._baseResourcePath;
			string relative = levelGraphPath.Replace('/', Path.DirectorySeparatorChar);
			string fullPath = Path.Combine(basePath, relative);

			if (!File.Exists(fullPath))
			{
				_log.Alert($"[TaskExecutorManager] LevelGraph file not found: {fullPath}");
				return null;
			}

			string json = File.ReadAllText(fullPath);
			return JsonConvert.DeserializeObject<LevelGraphConfig>(json, _jsonSettings);
		}
		catch (Exception ex)
		{
			_log.Alert($"[TaskExecutorManager] Failed to load LevelGraph '{levelGraphPath}': {ex.Message}");
			return null;
		}
	}

	private void ExecuteSequences(IEnumerable<LevelTaskSequence> sequences, TaskRuntime runtime)
	{
		if (sequences == null)
			return;

		foreach (var seq in sequences)
		{
			if (seq?.TaskList == null || seq.TaskList.Length == 0)
				continue;

			ExecuteTaskList(seq.TaskList.Where(t => t != null).ToList(), runtime);
		}
	}

	private void ExecuteTaskList(IReadOnlyList<TaskConfig> tasks, TaskRuntime runtime)
	{
		for (int i = 0; i < tasks.Count; i++)
		{
			var task = tasks[i];
			if (task == null)
				continue;

			if (task is WaitPredicateSucc wait)
			{
				RegisterWaitPredicate(wait, tasks.Skip(i + 1).Where(t => t != null).ToList(), runtime);
				break;
			}

			if (task is WaitCustomString waitCustom)
			{
				RegisterWaitCustomString(waitCustom, tasks.Skip(i + 1).Where(t => t != null).ToList(), runtime);
				break;
			}

			if (task is WaitSecond waitSecond)
			{
				ScheduleWaitSecond(waitSecond, tasks.Skip(i + 1).Where(t => t != null).ToList(), runtime);
				break;
			}

			ExecuteImmediateTask(task, runtime);
		}
	}

	private void RegisterWaitPredicate(WaitPredicateSucc wait, List<TaskConfig> nextTasks, TaskRuntime runtime)
	{
		if (wait.Condition == null)
		{
			_log.Alert("[TaskExecutorManager] WaitPredicateSucc has null Condition, ignoring");
			return;
		}

		_pendingPredicates.Add(new PendingWaitPredicate
		{
			WaitConfig = wait,
			NextTasks = nextTasks,
			Runtime = runtime,
		});

		_log.Message($"[TaskExecutorManager] Registered WaitPredicateSucc with condition {wait.Condition.GetType().FullName} for GroupId={runtime.GroupId}");
	}

	private void RegisterWaitCustomString(WaitCustomString config, List<TaskConfig> nextTasks, TaskRuntime runtime)
	{
		if (string.IsNullOrEmpty(config.CustomString))
		{
			_log.Alert("[TaskExecutorManager] WaitCustomString missing CustomString, ignoring");
			return;
		}

		_pendingWaitCustomStrings.Add(new PendingWaitCustomString
		{
			CustomString = config.CustomString,
			WaitOwnerOnly = config.WaitOwnerOnly,
			NextTasks = nextTasks,
			Runtime = runtime,
		});

		_log.Message($"[TaskExecutorManager] Registered WaitCustomString CustomString='{config.CustomString}', WaitOwnerOnly={config.WaitOwnerOnly}, GroupId={runtime.GroupId}, OwnerEntityId={runtime.OwnerPropEntityId}");
	}

	private void ScheduleWaitSecond(WaitSecond config, List<TaskConfig> nextTasks, TaskRuntime runtime)
	{
		int delayMs = (int)(config.WaitTime * 1000f);
		if (delayMs < 0)
		{
			delayMs = 0;
		}

		_log.Message($"[TaskExecutorManager] Scheduling WaitSecond for {delayMs} ms (GroupId={runtime.GroupId}, OwnerEntityId={runtime.OwnerPropEntityId})");

		_ = Task.Run(async () =>
		{
			try
			{
				await Task.Delay(delayMs).ConfigureAwait(false);
				ExecuteTaskList(nextTasks, runtime);
			}
			catch (Exception ex)
			{
				_log.Alert($"[TaskExecutorManager] Exception in WaitSecond continuation: {ex.Message}");
			}
		});
	}

	private void ExecuteImmediateTask(TaskConfig task, TaskRuntime runtime)
	{
		switch (task)
		{
			case PredicateTaskList predicateList:
				ExecutePredicateTaskList(predicateList, runtime);
				break;

			case AdventureTriggerAttack triggerAttack:
				ExecuteAdventureTriggerAttack(triggerAttack, runtime);
				break;

			case AdventureModifyTeamPlayerHP modifyHp:
				ExecuteAdventureModifyTeamPlayerHP(modifyHp, runtime);
				break;

			case AdventureModifyMazeMP modifyMazeMP:
				ExecuteAdventureModifyMazeMP(modifyMazeMP, runtime);
				break;

			case AdventureFireProjectile fireProjectile:
				ExecuteAdventureFireProjectile(fireProjectile, runtime);
				break;

			case ChangePropState change:
				ExecuteChangePropState(change);
				break;

			case SyncSubPropState syncSub:
				ExecuteSyncSubPropState(syncSub);
				break;

			case SyncAllSubPropState syncAll:
				ExecuteSyncAllSubPropState(syncAll);
				break;

			case LoopWaitBeHit loopBeHit:
				RegisterLoopWaitBeHit(loopBeHit, runtime.GroupId);
				break;

			case ComparePropState compare:
				ExecuteComparePropState(compare, runtime.GroupId);
				break;

			case ReversePropState reverse:
				ExecuteReversePropState(reverse);
				break;

			case ToastPile toast:
				ExecuteToastPile(toast);
				break;

			case EnterMap enterMap:
				ExecuteEnterMap(enterMap);
				break;

			case AddMazeBuff addMazeBuff:
				ExecuteAddMazeBuff(addMazeBuff, runtime);
				break;

			case RemoveMazeBuff removeMazeBuff:
				ExecuteRemoveMazeBuff(removeMazeBuff, runtime);
				break;

			default:
				_log.Message($"[TaskExecutorManager] Unhandled task type: {task.GetType().FullName}");
				break;
		}
	}
}