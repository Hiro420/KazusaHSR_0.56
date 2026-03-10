using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.Protocol;
using KazusaHSR.Resource;
using Newtonsoft.Json;

namespace KazusaHSR.GameServer;

public sealed class SceneLevelGraphExecutor
{
	private readonly Scene _scene;
	private readonly Session _session;
	private readonly Logger _log;

	private readonly List<PendingWaitPredicate> _pending = new();
	private readonly List<PendingWaitCustomString> _pendingWaitCustomStrings = new();

	private sealed class PendingBeHit
	{
		public uint GroupId { get; init; }
		public uint PropInstId { get; init; }
		public List<TaskConfig> OnBeHitTasks { get; init; } = new();
	}

	private readonly List<PendingBeHit> _pendingBeHits = new();

	private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
	{
		Converters = new List<JsonConverter> { new TaskConfigJsonConverter() }
	};

	private sealed class PendingWaitPredicate
	{
		public WaitPredicateSucc WaitConfig { get; init; } = null!;
		public List<TaskConfig> NextTasks { get; init; } = new();
		public uint GroupId { get; init; }
		public uint OwnerPropEntityId { get; init; }
	}

	private sealed class PendingWaitCustomString
	{
		public string CustomString { get; init; } = string.Empty;
		public bool WaitOwnerOnly { get; init; }
		public uint GroupId { get; init; }
		public uint OwnerPropEntityId { get; init; }
		public List<TaskConfig> NextTasks { get; init; } = new();
	}

	public SceneLevelGraphExecutor(Scene scene)
	{
		_scene = scene ?? throw new ArgumentNullException(nameof(scene));
		_session = scene.session ?? throw new ArgumentNullException(nameof(scene.session));
		_log = _session.c;
	}

	public void StartForGroup(uint groupId, LevelGroupInfo groupInfo)
	{
		if (groupInfo == null)
			return;

		if (string.IsNullOrEmpty(groupInfo.LevelGraph))
			return;

		if (string.IsNullOrEmpty(groupInfo.LevelGraph))
			return;

		StartLevelGraph(groupInfo.LevelGraph, groupId, 0, $"group {groupInfo.GroupGUID}");
	}

	public void StartInitGraphForProp(PropEntity prop)
	{
		if (prop == null)
			return;

		string path = prop.DbInfo.InitLevelGraph;
		if (string.IsNullOrEmpty(path))
			return;

		StartLevelGraph(path, prop.GroupId, prop._EntityId, $"prop GroupId={prop.GroupId}, InstId={prop.DbInfo.ID}");
	}

	private void StartLevelGraph(string levelGraphPath, uint groupId, uint ownerPropEntityId, string context)
	{
		try
		{
			var config = LoadLevelGraph(levelGraphPath);
			if (config == null)
			{
				_log.LogWarning($"[SceneLevelGraphExecutor] Failed to load LevelGraph '{levelGraphPath}' for {context}");
				return;
			}

			_log.LogInfo($"[SceneLevelGraphExecutor] Starting LevelGraph '{levelGraphPath}' for {context} (GroupId={groupId}, OwnerEntityId={ownerPropEntityId})");

			ExecuteSequences(config.OnInitSequece ?? Array.Empty<LevelTaskSequence>(), groupId, ownerPropEntityId);
			ExecuteSequences(config.OnStartSequece ?? Array.Empty<LevelTaskSequence>(), groupId, ownerPropEntityId);
		}
		catch (Exception ex)
		{
			_log.LogWarning($"[SceneLevelGraphExecutor] Exception while loading LevelGraph '{levelGraphPath}' for {context}: {ex.Message}");
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
				_log.LogWarning($"[SceneLevelGraphExecutor] LevelGraph file not found: {fullPath}");
				return null;
			}

			string json = File.ReadAllText(fullPath);
			var config = JsonConvert.DeserializeObject<LevelGraphConfig>(json, _jsonSettings);
			return config;
		}
		catch (Exception ex)
		{
			_log.LogWarning($"[SceneLevelGraphExecutor] Failed to load LevelGraph '{levelGraphPath}': {ex.Message}");
			return null;
		}
	}

	private void ExecuteSequences(IEnumerable<LevelTaskSequence> sequences, uint groupId)
	{
		ExecuteSequences(sequences, groupId, 0);
	}

	private void ExecuteSequences(IEnumerable<LevelTaskSequence> sequences, uint groupId, uint ownerPropEntityId)
	{
		if (sequences == null)
			return;

		foreach (var seq in sequences)
		{
			if (seq?.TaskList == null || seq.TaskList.Length == 0)
				continue;

			ExecuteTaskList(seq.TaskList, groupId, ownerPropEntityId);
		}
	}

	private void ExecuteTaskList(IReadOnlyList<TaskConfig> tasks, uint groupId)
	{
		ExecuteTaskList(tasks, groupId, 0);
	}

	private void ExecuteTaskList(IReadOnlyList<TaskConfig> tasks, uint groupId, uint ownerPropEntityId)
	{
		for (int i = 0; i < tasks.Count; i++)
		{
			var task = tasks[i];
			if (task == null)
				continue;

			if (task is WaitPredicateSucc wait)
			{
				var nextTasks = tasks.Skip(i + 1).Where(t => t != null).ToList();
				RegisterWaitPredicate(wait, nextTasks, groupId, ownerPropEntityId);
				break;
			}
			else if (task is WaitCustomString waitCustom)
			{
				var nextTasks = tasks.Skip(i + 1).Where(t => t != null).ToList();
				RegisterWaitCustomString(waitCustom, nextTasks, groupId, ownerPropEntityId);
				break;
			}
			else if (task is WaitSecond waitSecond)
			{
				var nextTasks = tasks.Skip(i + 1).Where(t => t != null).ToList();
				ScheduleWaitSecond(waitSecond, nextTasks, groupId, ownerPropEntityId);
				break;
			}
			else
			{
				ExecuteImmediateTask(task, groupId, ownerPropEntityId);
			}
		}
	}

	private void RegisterWaitPredicate(WaitPredicateSucc wait, List<TaskConfig> nextTasks, uint groupId, uint ownerPropEntityId)
	{
		if (wait.Condition == null)
		{
			_log.LogWarning("[SceneLevelGraphExecutor] WaitPredicateSucc has null Condition, ignoring");
			return;
		}

		var pending = new PendingWaitPredicate
		{
			WaitConfig = wait,
			NextTasks = nextTasks,
			GroupId = groupId,
			OwnerPropEntityId = ownerPropEntityId,
		};

		_pending.Add(pending);
		_log.LogInfo($"[SceneLevelGraphExecutor] Registered WaitPredicateSucc with condition {wait.Condition.GetType().FullName} for GroupId={groupId}");
	}

	private void RegisterWaitCustomString(WaitCustomString config, List<TaskConfig> nextTasks, uint groupId, uint ownerPropEntityId)
	{
		if (string.IsNullOrEmpty(config.CustomString))
		{
			_log.LogWarning("[SceneLevelGraphExecutor] WaitCustomString missing CustomString, ignoring");
			return;
		}

		var pending = new PendingWaitCustomString
		{
			CustomString = config.CustomString,
			WaitOwnerOnly = config.WaitOwnerOnly,
			GroupId = groupId,
			OwnerPropEntityId = ownerPropEntityId,
			NextTasks = nextTasks,
		};

		_pendingWaitCustomStrings.Add(pending);
		_log.LogInfo($"[SceneLevelGraphExecutor] Registered WaitCustomString CustomString='{config.CustomString}', WaitOwnerOnly={config.WaitOwnerOnly}, GroupId={groupId}, OwnerEntityId={ownerPropEntityId}");
	}

	private void ScheduleWaitSecond(WaitSecond config, List<TaskConfig> nextTasks, uint groupId, uint ownerPropEntityId)
	{
		int delayMs = (int)(config.WaitTime * 1000f);
		if (delayMs < 0)
		{
			delayMs = 0;
		}

		_log.LogInfo($"[SceneLevelGraphExecutor] Scheduling WaitSecond for {delayMs} ms (GroupId={groupId}, OwnerEntityId={ownerPropEntityId})");

		_ = Task.Run(async () =>
		{
			try
			{
				await Task.Delay(delayMs).ConfigureAwait(false);
				ExecuteTaskList(nextTasks, groupId, ownerPropEntityId);
			}
			catch (Exception ex)
			{
				_log.LogWarning($"[SceneLevelGraphExecutor] Exception in WaitSecond continuation: {ex.Message}");
			}
		});
	}

	private void ExecuteImmediateTask(TaskConfig task, uint groupId, uint ownerPropEntityId)
	{
		switch (task)
		{
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
				RegisterLoopWaitBeHit(loopBeHit, groupId);
				break;

			case ComparePropState compare:
				ExecuteComparePropState(compare, groupId);
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
				ExecuteAddMazeBuff(addMazeBuff, ownerPropEntityId);
				break;

			case RemoveMazeBuff removeMazeBuff:
				ExecuteRemoveMazeBuff(removeMazeBuff, ownerPropEntityId);
				break;

			// todo AdventureModify for HP, SP etc
			//case AdventureModifyTeamPlayerSP adventureModifyTeamPlayerSP:
			//	ExecuteAdventureModifyTeamPlayerSP(adventureModifyTeamPlayerSP);
			//	break;

			default:
				_log.LogInfo($"[SceneLevelGraphExecutor] Unhandled task type in level graph: {task.GetType().FullName}");
				break;
		}
	}

	private List<BaseEntity> ResolveTargetType(TargetEvaluator targetType, uint ownerPropEntityId)
	{
		List<BaseEntity> result = new();

		if (targetType == null || targetType is not TargetAlias targetAlias)
		{
			// all avatars in lineup
			IEnumerable<PlayerAvatar> teamAvatars = _session.player.ChallengeManager.IsInChallenge ?
					_session.player.ChallengeManager.VirtualLineup.AvatarIds.Where(id => id != 0).Select(id => _session.player.avatarDict.Values.First(a => a.AvatarId == id))
					: _session.player.GetCurrentLineup().Avatars;
			result.AddRange(teamAvatars
				.Select(_session.player.FindEntityByPlayerAvatar)
				.Where(e => e != null)
				.Cast<BaseEntity>()
				.ToList()
			);
			return result;
		}

		_session.c.LogInfo($"[SceneLevelGraphExecutor] Resolving TargetAlias: {targetAlias.Alias}");

		switch (targetAlias.Alias)
		{
			case "TaskActionTarget":
				if (_scene.EntityManager.TryGet(ownerPropEntityId, out BaseEntity? target))
				{
					result.Add(target);
				}
				break;
			case "LineupLeader":
				if (_session.player.GetCurrentLineup().Leader == null)
				{
					_log.LogWarning("[SceneLevelGraphExecutor] LineupLeader alias used but player has no lineup leader");
					return [];
				}
				AvatarEntity? leader = _session.player.FindEntityByPlayerAvatar(_session.player.GetCurrentLineup().Leader!);
				if (leader != null)
				{
					return new() { leader };
				}
				break;
			case "LightTeamEntity":
				// all avatars in lineup
				IEnumerable<PlayerAvatar> teamAvatars = _session.player.ChallengeManager.IsInChallenge ?
						_session.player.ChallengeManager.VirtualLineup.AvatarIds.Select(id => _session.player.avatarDict.Values.First(a => a.AvatarId == id)).Where(a => a != null)!
						: _session.player.GetCurrentLineup().Avatars;
				result.AddRange(teamAvatars
					.Select(_session.player.FindEntityByPlayerAvatar)
					.Where(e => e != null)
					.Cast<BaseEntity>()
					.ToList()
				);
				break;
			default:
				_log.LogWarning($"[SceneLevelGraphExecutor] Unknown TargetAlias: {targetAlias.Alias}, falling back to LineupLeader");
				goto case "LineupLeader"; // fallback to LineupLeader for unknown aliases
		}

		return result;
	}

	private void ExecuteRemoveMazeBuff(RemoveMazeBuff config, uint ownerPropEntityId)
	{
		if (config.ID == 0)
		{
			_log.LogWarning("[SceneLevelGraphExecutor] RemoveMazeBuff missing BuffID");
			return;
		}
		List<BaseEntity> resolvedTargets = ResolveTargetType(config.TargetType, ownerPropEntityId);
		if (resolvedTargets == null || !resolvedTargets.Any())
		{
			_log.LogWarning("[SceneLevelGraphExecutor] RemoveMazeBuff failed: could not find entity for lineup leader");
			return;
		}
		foreach (var target in resolvedTargets)
		{
			target.RemoveMazeBuff(config.ID, resolvedTargets.Count() == 1);
		}
	}

	public void ExecuteAddMazeBuff(AddMazeBuff config, uint ownerPropEntityId)
	{
		if (config.ID == 0)
		{
			_log.LogWarning("[SceneLevelGraphExecutor] AddMazeBuff missing BuffID");
			return;
		}
		List<BaseEntity> resolvedTargets = ResolveTargetType(config.TargetType, ownerPropEntityId);
		if (resolvedTargets == null || !resolvedTargets.Any())
		{
			_log.LogWarning("[SceneLevelGraphExecutor] AddMazeBuff failed: could not find valid targets");
			return;
		}
		foreach (var target in resolvedTargets)
		{
			target.AddMazeBuff(config.ID, resolvedTargets.Count() == 1);
		}
	}

	public void ExecuteEnterMap(EnterMap config)
	{
		Resource.Excel.MapEntryRow? entryRow = MainApp.resourceManager.MapEntranceExcel.Find(a => a.ID == config.EntranceID);
		if (entryRow == null)
		{
			_log.LogError($"[SceneLevelGraphExecutor] EnterMap: MapEntry not found for EntranceID={config.EntranceID}");
			return;
		}

		_session.player.EnterMazeByGroupAnchor(entryRow, config.GroupID, config.AnchorID, out Maze? maze);
		if (maze == null)
		{
			_log.LogError($"[SceneLevelGraphExecutor] EnterMap: Failed to enter maze for EntranceID={config.EntranceID}, GroupID={config.GroupID}, AnchorID={config.AnchorID}");
			return;
		}
		EnterMazeByServerScNotify notify = new EnterMazeByServerScNotify
		{
			Maze = maze,
		};
		_session.SendPacket(notify);
		_log.LogInfo($"[SceneLevelGraphExecutor] EnterMap: Player entered maze for EntranceID={config.EntranceID}, GroupID={entryRow.EntranceGroupID}, AnchorID={config.AnchorID}, MazeFloor={maze.Floor}, MapEntryId={maze.MapEntryId}");
		_session.player.SavePersistent();
	}

	private void ExecuteChangePropState(ChangePropState config)
	{
		if (config.DynamicGroupID == null || config.DynamicGroupPropID == null)
		{
			_log.LogWarning("[SceneLevelGraphExecutor] ChangePropState missing DynamicGroupID or DynamicGroupPropID");
			return;
		}

		uint groupId = (uint)config.DynamicGroupID.Evaluate();
		uint instId = (uint)config.DynamicGroupPropID.Evaluate();

		var propEntity = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == instId);

		if (propEntity == null)
		{
			_log.LogWarning($"[SceneLevelGraphExecutor] ChangePropState: Prop not found for GroupId={groupId}, InstId={instId}");
			return;
		}

		propEntity.State = config.State;

		var notify = new SceneEntityUpdateScNotify();
		notify.EntityLists.Add(propEntity.ToSceneEntityInfo());
		_session.SendPacket(notify);

		_log.LogInfo($"[SceneLevelGraphExecutor] ChangePropState applied: GroupId={groupId}, InstId={instId}, NewState={config.State}");

		OnMonstersChanged();
	}

	private void ExecuteSyncSubPropState(SyncSubPropState config)
	{
		if (config.MainGroupID == null || config.MainGroupPropID == null ||
			config.SubGroupID == null || config.SubGroupPropID == null)
		{
			_log.LogWarning("[SceneLevelGraphExecutor] SyncSubPropState missing IDs");
			return;
		}

		uint mainGroupId = (uint)config.MainGroupID.Evaluate();
		uint mainPropId = (uint)config.MainGroupPropID.Evaluate();
		uint subGroupId = (uint)config.SubGroupID.Evaluate();
		uint subPropId = (uint)config.SubGroupPropID.Evaluate();

		var mainProp = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == mainGroupId && p.DbInfo.ID == mainPropId);
		if (mainProp == null)
		{
			_log.LogWarning($"[SceneLevelGraphExecutor] SyncSubPropState: main prop not found for GroupId={mainGroupId}, InstId={mainPropId}");
			return;
		}

		if (mainProp.State != config.MainState)
		{
			return;
		}

		var subProp = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == subGroupId && p.DbInfo.ID == subPropId);
		if (subProp == null)
		{
			_log.LogWarning($"[SceneLevelGraphExecutor] SyncSubPropState: sub prop not found for GroupId={subGroupId}, InstId={subPropId}");
			return;
		}

		subProp.State = config.SubState;

		var notify = new SceneEntityUpdateScNotify();
		notify.EntityLists.Add(subProp.ToSceneEntityInfo());
		_session.SendPacket(notify);

		_log.LogInfo($"[SceneLevelGraphExecutor] SyncSubPropState applied: Main({mainGroupId},{mainPropId}) -> Sub({subGroupId},{subPropId}) NewState={config.SubState}");

		OnMonstersChanged();
	}

	private void ExecuteSyncAllSubPropState(SyncAllSubPropState config)
	{
		if (config.GroupID == null || config.GroupPropID == null)
		{
			_log.LogWarning("[SceneLevelGraphExecutor] SyncAllSubPropState missing IDs");
			return;
		}

		uint groupId = (uint)config.GroupID.Evaluate();
		uint propId = (uint)config.GroupPropID.Evaluate();

		var mainProp = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == propId);
		if (mainProp == null)
		{
			_log.LogWarning($"[SceneLevelGraphExecutor] SyncAllSubPropState: main prop not found for GroupId={groupId}, InstId={propId}");
			return;
		}

		var subProps = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.Where(p => p.GroupId == groupId && p.DbInfo.PropID == mainProp.DbInfo.PropID && p.DbInfo.ID != mainProp.DbInfo.ID)
			.ToList();

		if (subProps.Count == 0)
			return;

		var notify = new SceneEntityUpdateScNotify();
		foreach (var sub in subProps)
		{
			sub.State = mainProp.State;
			notify.EntityLists.Add(sub.ToSceneEntityInfo());
		}
		_session.SendPacket(notify);

		_log.LogInfo($"[SceneLevelGraphExecutor] SyncAllSubPropState applied for GroupId={groupId}, MainInstId={propId}, Count={subProps.Count}");

		OnMonstersChanged();
	}

	private void RegisterLoopWaitBeHit(LoopWaitBeHit config, uint groupId)
	{
		if (config.GroupPropID == null || config.OnBeHit == null || config.OnBeHit.Length == 0)
		{
			_log.LogWarning("[SceneLevelGraphExecutor] LoopWaitBeHit missing GroupPropID or OnBeHit tasks");
			return;
		}

		uint instId = (uint)config.GroupPropID.Evaluate();

		var prop = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == instId);
		if (prop == null)
		{
			_log.LogWarning($"[SceneLevelGraphExecutor] LoopWaitBeHit: prop not found for GroupId={groupId}, InstId={instId}");
			return;
		}

		var existing = _pendingBeHits.FirstOrDefault(p => p.GroupId == groupId && p.PropInstId == instId);
		if (existing != null)
		{
			foreach (var t in config.OnBeHit)
			{
				if (t != null)
				{
					existing.OnBeHitTasks.Add(t);
				}
			}
		}
		else
		{
			var tasks = config.OnBeHit.Where(t => t != null).ToList();
			_pendingBeHits.Add(new PendingBeHit
			{
				GroupId = groupId,
				PropInstId = instId,
				OnBeHitTasks = tasks,
			});
		}

		_log.LogInfo($"[SceneLevelGraphExecutor] Registered LoopWaitBeHit for GroupId={groupId}, InstId={instId}, TaskCount={config.OnBeHit.Length}");
	}

	private void ExecuteComparePropState(ComparePropState config, uint fallbackGroupId)
	{
		if (config.GroupID == null || config.GroupPropID == null)
		{
			_log.LogWarning("[SceneLevelGraphExecutor] ComparePropState missing IDs");
			return;
		}

		uint groupId = (uint)config.GroupID.Evaluate();
		uint instId = (uint)config.GroupPropID.Evaluate();

		var prop = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == instId);
		if (prop == null)
		{
			_log.LogWarning($"[SceneLevelGraphExecutor] ComparePropState: prop not found for GroupId={groupId}, InstId={instId}");
			return;
		}

		bool equal = prop.State == config.State;
		TaskConfig[]? next = equal ? config.OnEqual : config.OnNotEqual;
		if (next == null || next.Length == 0)
			return;

		uint nextGroupId = groupId != 0 ? groupId : fallbackGroupId;
		ExecuteTaskList(next, nextGroupId);
	}

	private void ExecuteReversePropState(ReversePropState config)
	{
		if (config.GroupID == null || config.GroupPropID == null)
		{
			_log.LogWarning("[SceneLevelGraphExecutor] ReversePropState missing IDs");
			return;
		}

		uint groupId = (uint)config.GroupID.Evaluate();
		uint instId = (uint)config.GroupPropID.Evaluate();

		var prop = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == instId);
		if (prop == null)
		{
			_log.LogWarning($"[SceneLevelGraphExecutor] ReversePropState: prop not found for GroupId={groupId}, InstId={instId}");
			return;
		}

		PropState oldState = prop.State;
		PropState newState = oldState == PropState.Open ? PropState.Closed : PropState.Open;
		prop.State = newState;

		var notify = new SceneEntityUpdateScNotify();
		notify.EntityLists.Add(prop.ToSceneEntityInfo());
		_session.SendPacket(notify);

		_log.LogInfo($"[SceneLevelGraphExecutor] ReversePropState applied for GroupId={groupId}, InstId={instId}, {oldState} -> {newState}");

		OnMonstersChanged();
	}

	private void ExecuteToastPile(ToastPile config)
	{
		// TODO?
		_log.LogInfo($"[SceneLevelGraphExecutor] ToastPile: ImgPath={config.ImgPath}, DescTextID={config.DescTextID}");
	}

	public void OnWaitCustomStringReceived(string customString, uint propEntityId, uint subMissionId)
	{
		if (_pendingWaitCustomStrings.Count == 0)
			return;

		var matches = _pendingWaitCustomStrings
			.Where(p => p.CustomString == customString &&
				(!p.WaitOwnerOnly || (propEntityId != 0 && p.OwnerPropEntityId == propEntityId)))
			.ToList();
		if (matches.Count == 0)
			return;

		foreach (var pending in matches)
		{
			_pendingWaitCustomStrings.Remove(pending);
			if (pending.NextTasks != null && pending.NextTasks.Count > 0)
			{
				ExecuteTaskList(pending.NextTasks, pending.GroupId, pending.OwnerPropEntityId);
			}
			_log.LogInfo($"[SceneLevelGraphExecutor] WaitCustomString satisfied for CustomString='{pending.CustomString}', GroupId={pending.GroupId}, OwnerEntityId={pending.OwnerPropEntityId}, PropEntityId={propEntityId}, SubMissionId={subMissionId}");
		}
	}

	private bool EvaluatePredicate(PredicateConfig? predicate)
	{
		if (predicate == null)
			return false;

		switch (predicate)
		{
			case ByCompareGroupMonsterNum byMonster:
				{
					uint groupId = byMonster.GroupID != 0 ? byMonster.GroupID : 0;
					int currentCount = _scene.EntityManager.Entities.Values
						.OfType<MonsterEntity>()
						.Count(m => groupId == 0 || m.GroupId == groupId);
					return currentCount == byMonster.Number;
				}

			case ByComparePropState byProp:
				{
					uint groupId = byProp.GroupID;
					uint propId = byProp.GroupPropID;
					var propEntity = _scene.EntityManager.Entities.Values
						.OfType<PropEntity>()
						.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == propId);

					if (propEntity == null)
						return false;

					return propEntity.State == byProp.State;
				}

			case ByAnd byAnd:
				{
					if (byAnd.PredicateList == null || byAnd.PredicateList.Length == 0)
						return false;
					foreach (var child in byAnd.PredicateList)
					{
						if (!EvaluatePredicate(child))
							return false;
					}
					return true;
				}

			case ByAny byAny:
				{
					if (byAny.PredicateList == null || byAny.PredicateList.Length == 0)
						return false;
					foreach (var child in byAny.PredicateList)
					{
						if (EvaluatePredicate(child))
							return true;
					}
					return false;
				}

			default:
				_log.LogInfo($"[SceneLevelGraphExecutor] EvaluatePredicate: unsupported predicate type {predicate.GetType().FullName}");
				return false;
		}
	}

	public void OnPropBeHit(PropEntity prop)
	{
		if (prop == null)
			return;

		if (_pendingBeHits.Count == 0)
			return;

		uint groupId = prop.GroupId;
		uint instId = prop.DbInfo.ID;

		var matches = _pendingBeHits
			.Where(p => p.GroupId == groupId && p.PropInstId == instId)
			.ToList();
		if (matches.Count == 0)
			return;

		foreach (var pending in matches)
		{
			if (pending.OnBeHitTasks == null || pending.OnBeHitTasks.Count == 0)
				continue;

			ExecuteTaskList(pending.OnBeHitTasks, groupId);
		}

		_log.LogInfo($"[SceneLevelGraphExecutor] OnPropBeHit triggered for GroupId={groupId}, InstId={instId}, Count={matches.Count}");
	}

	public void OnMonstersChanged()
	{
		if (_pending.Count == 0)
			return;

		var snapshot = _pending.ToList();
		foreach (var pending in snapshot)
		{
			var cond = pending.WaitConfig.Condition;
			if (!EvaluatePredicate(cond))
				continue;

			uint groupId = pending.GroupId;
			_log.LogInfo($"[SceneLevelGraphExecutor] WaitPredicateSucc satisfied for GroupId={groupId} with condition {cond?.GetType().FullName}");
			_pending.Remove(pending);

			if (pending.NextTasks != null && pending.NextTasks.Count > 0)
			{
				ExecuteTaskList(pending.NextTasks, groupId, pending.OwnerPropEntityId);
			}
		}
	}
}
