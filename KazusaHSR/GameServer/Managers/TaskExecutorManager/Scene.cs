using KazusaHSR.GameServer.Resource;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public sealed partial class TaskExecutorManager
{
	private void ExecuteEnterMap(EnterMap config)
	{
		Resource.Excel.MapEntryRow? entryRow = MainApp.resourceManager.MapEntranceExcel.Find(a => a.ID == config.EntranceID);
		if (entryRow == null)
		{
			_log.Fail($"[TaskExecutorManager] EnterMap: MapEntry not found for EntranceID={config.EntranceID}");
			return;
		}

		_session.player.EnterMazeByGroupAnchor(entryRow, config.GroupID, config.AnchorID, out Maze? maze);
		if (maze == null)
		{
			_log.Fail($"[TaskExecutorManager] EnterMap: Failed to enter maze for EntranceID={config.EntranceID}, GroupID={config.GroupID}, AnchorID={config.AnchorID}");
			return;
		}

		var notify = new EnterMazeByServerScNotify { Maze = maze };
		_session.SendPacket(notify);
		_session.player.SavePersistent();
	}

	private void ExecuteChangePropState(ChangePropState config)
	{
		if (config.DynamicGroupID == null || config.DynamicGroupPropID == null)
		{
			_log.Alert("[TaskExecutorManager] ChangePropState missing DynamicGroupID or DynamicGroupPropID");
			return;
		}

		uint groupId = (uint)config.DynamicGroupID.Evaluate();
		uint instId = (uint)config.DynamicGroupPropID.Evaluate();

		var propEntity = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == instId);
		if (propEntity == null)
		{
			_log.Alert($"[TaskExecutorManager] ChangePropState: Prop not found for GroupId={groupId}, InstId={instId}");
			return;
		}

		propEntity.State = config.State;

		var notify = new SceneEntityUpdateScNotify();
		notify.EntityLists.Add(propEntity.ToSceneEntityInfo());
		_session.SendPacket(notify);
		OnMonstersChanged();
	}

	private void ExecuteSyncSubPropState(SyncSubPropState config)
	{
		if (config.MainGroupID == null || config.MainGroupPropID == null ||
			config.SubGroupID == null || config.SubGroupPropID == null)
		{
			_log.Alert("[TaskExecutorManager] SyncSubPropState missing IDs");
			return;
		}

		uint mainGroupId = (uint)config.MainGroupID.Evaluate();
		uint mainPropId = (uint)config.MainGroupPropID.Evaluate();
		uint subGroupId = (uint)config.SubGroupID.Evaluate();
		uint subPropId = (uint)config.SubGroupPropID.Evaluate();

		var mainProp = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == mainGroupId && p.DbInfo.ID == mainPropId);
		if (mainProp == null || mainProp.State != config.MainState)
			return;

		var subProp = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == subGroupId && p.DbInfo.ID == subPropId);
		if (subProp == null)
		{
			_log.Alert($"[TaskExecutorManager] SyncSubPropState: sub prop not found for GroupId={subGroupId}, InstId={subPropId}");
			return;
		}

		subProp.State = config.SubState;

		var notify = new SceneEntityUpdateScNotify();
		notify.EntityLists.Add(subProp.ToSceneEntityInfo());
		_session.SendPacket(notify);
		OnMonstersChanged();
	}

	private void ExecuteSyncAllSubPropState(SyncAllSubPropState config)
	{
		if (config.GroupID == null || config.GroupPropID == null)
		{
			_log.Alert("[TaskExecutorManager] SyncAllSubPropState missing IDs");
			return;
		}

		uint groupId = (uint)config.GroupID.Evaluate();
		uint propId = (uint)config.GroupPropID.Evaluate();

		var mainProp = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == propId);
		if (mainProp == null)
		{
			_log.Alert($"[TaskExecutorManager] SyncAllSubPropState: main prop not found for GroupId={groupId}, InstId={propId}");
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
		OnMonstersChanged();
	}

	private void RegisterLoopWaitBeHit(LoopWaitBeHit config, uint groupId)
	{
		if (config.GroupPropID == null || config.OnBeHit == null || config.OnBeHit.Length == 0)
		{
			_log.Alert("[TaskExecutorManager] LoopWaitBeHit missing GroupPropID or OnBeHit tasks");
			return;
		}

		uint instId = (uint)config.GroupPropID.Evaluate();
		var prop = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == instId);
		if (prop == null)
		{
			_log.Alert($"[TaskExecutorManager] LoopWaitBeHit: prop not found for GroupId={groupId}, InstId={instId}");
			return;
		}

		var existing = _pendingBeHits.FirstOrDefault(p => p.GroupId == groupId && p.PropInstId == instId);
		if (existing != null)
		{
			existing.OnBeHitTasks.AddRange(config.OnBeHit.Where(t => t != null));
		}
		else
		{
			_pendingBeHits.Add(new PendingBeHit
			{
				GroupId = groupId,
				PropInstId = instId,
				OnBeHitTasks = config.OnBeHit.Where(t => t != null).ToList(),
			});
		}
	}

	private void ExecuteComparePropState(ComparePropState config, uint fallbackGroupId)
	{
		if (config.GroupID == null || config.GroupPropID == null)
		{
			_log.Alert("[TaskExecutorManager] ComparePropState missing IDs");
			return;
		}

		uint groupId = (uint)config.GroupID.Evaluate();
		uint instId = (uint)config.GroupPropID.Evaluate();

		var prop = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == instId);
		if (prop == null)
		{
			_log.Alert($"[TaskExecutorManager] ComparePropState: prop not found for GroupId={groupId}, InstId={instId}");
			return;
		}

		TaskConfig[]? next = prop.State == config.State ? config.OnEqual : config.OnNotEqual;
		if (next == null || next.Length == 0)
			return;

		uint nextGroupId = groupId != 0 ? groupId : fallbackGroupId;
		ExecuteTaskList(next.Where(t => t != null).ToList(), new TaskRuntime
		{
			Kind = RuntimeKind.Scene,
			GroupId = nextGroupId,
			OwnerPropEntityId = 0,
			AdventureContext = null,
		});
	}

	private void ExecuteReversePropState(ReversePropState config)
	{
		if (config.GroupID == null || config.GroupPropID == null)
		{
			_log.Alert("[TaskExecutorManager] ReversePropState missing IDs");
			return;
		}

		uint groupId = (uint)config.GroupID.Evaluate();
		uint instId = (uint)config.GroupPropID.Evaluate();

		var prop = _scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == instId);
		if (prop == null)
		{
			_log.Alert($"[TaskExecutorManager] ReversePropState: prop not found for GroupId={groupId}, InstId={instId}");
			return;
		}

		prop.State = prop.State == PropState.Open ? PropState.Closed : PropState.Open;

		var notify = new SceneEntityUpdateScNotify();
		notify.EntityLists.Add(prop.ToSceneEntityInfo());
		_session.SendPacket(notify);
		OnMonstersChanged();
	}

	private void ExecuteToastPile(ToastPile config)
	{
		_log.Message($"[TaskExecutorManager] ToastPile: ImgPath={config.ImgPath}, DescTextID={config.DescTextID}");
	}
}