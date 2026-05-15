using KazusaHSR.GameServer.Resource;

namespace KazusaHSR.GameServer;

public sealed partial class TaskExecutorManager
{
	private void ExecuteAdventureTriggerAttack(AdventureTriggerAttack config, TaskRuntime runtime)
	{
		var ctx = runtime.AdventureContext;
		if (ctx == null)
			return;

		var session = ctx.Session;
		var req = ctx.Request;
		var rsp = ctx.Response;
		var entities = session.player.Scene.EntityManager.Entities;

		IEnumerable<uint> hitMonsterIds = (req.HitTargetEntityIdLists ?? Array.Empty<uint>())
			.Distinct()
			.Where(id => entities.TryGetValue(id, out var ent) && ent is MonsterEntity);

		IEnumerable<uint> hitPropIds = (req.HitTargetEntityIdLists ?? Array.Empty<uint>())
			.Distinct()
			.Where(id => entities.TryGetValue(id, out var ent) && ent is PropEntity);

		IEnumerable<uint> assistMonsterIds = (req.AssistMonsterEntityIdLists ?? Array.Empty<uint>())
			.Distinct()
			.Where(id => entities.TryGetValue(id, out var ent) && ent is MonsterEntity);

		if (hitMonsterIds.Any())
		{
			if (config.OnAttack != null)
			{
				ExecuteTaskList(config.OnAttack.Where(t => t != null).ToList(), runtime);
			}

			session.player.battleManager.StartMonsterBattle(hitMonsterIds, assistMonsterIds);
			rsp.BattleInfo = session.player.battleManager.GetCurrentBattleInfo();
		}

		foreach (uint propGuid in hitPropIds)
		{
			if (entities[propGuid] is not PropEntity propEntity)
				continue;

			AvatarEntity? currentAvatar = session.player.GetCurrentLineup().Leader != null ?
				session.player.Scene.EntityManager.TryGetByPlayerAvatar(session.player.GetCurrentLineup().Leader!) : null;
			OnPropBeHit(propEntity, currentAvatar);
		}
	}

	private void ExecuteAdventureModifyTeamPlayerHP(AdventureModifyTeamPlayerHP config, TaskRuntime runtime)
	{
		var ctx = runtime.AdventureContext;
		if (ctx == null)
			return;
		var session = ctx.Session;
		// TODO: other types
		if (config.AddRatio != null)
		{
			double ratio = config.AddRatio.Evaluate(ctx);
			// the value is like 0.35, so we restore 35% of max HP
			session.player.TeamManager.ModifyTeamPlayerHP(player => (int)(player.MaxHp * ratio));
		}
	}

	private void ExecuteAdventureModifyMazeMP(AdventureModifyMazeMP config, TaskRuntime runtime)
	{
		var ctx = runtime.AdventureContext;
		if (ctx == null)
			return;

		if (config.ModifyFunction == IGNHFEOEBHO.Unknow || config.ModifyValue == null)
			return;

		var mpManager = ctx.Session.player.MPManager;
		double value = config.ModifyValue.Evaluate(ctx);
		uint maxMp = mpManager.GetMaxAmount();
		uint currentMp = mpManager.GetCurrentAmount();

		switch (config.ModifyFunction)
		{
			case IGNHFEOEBHO.Set:
				{
					uint target = (uint)Math.Clamp((int)Math.Round(value), 0, (int)maxMp);
					mpManager.SetAmount(target);
					break;
				}
			case IGNHFEOEBHO.Add:
				{
					int delta = (int)Math.Round(value);
					uint target = (uint)Math.Clamp((int)currentMp + delta, 0, (int)maxMp);
					mpManager.SetAmount(target);
					break;
				}
			case IGNHFEOEBHO.Mul:
				{
					uint target = (uint)Math.Clamp((int)Math.Round(currentMp * value), 0, (int)maxMp);
					mpManager.SetAmount(target);
					break;
				}
		}
	}

	private void ExecuteAdventureFireProjectile(AdventureFireProjectile config, TaskRuntime runtime)
	{
		var ctx = runtime.AdventureContext;
		if (ctx == null)
			return;

		bool isHit = ctx.Request.HitTargetEntityIdLists != null && ctx.Request.HitTargetEntityIdLists.Length > 0;
		if (isHit)
		{
			if (config.OnProjectileHit != null)
			{
				ExecuteTaskList(config.OnProjectileHit.Where(t => t != null).ToList(), runtime);
			}
			else if (config.OnProjectileLifetimeFinish != null)
			{
				ExecuteTaskList(config.OnProjectileLifetimeFinish.Where(t => t != null).ToList(), runtime);
			}
		}
	}

	private void ExecuteRemoveMazeBuff(RemoveMazeBuff config, TaskRuntime runtime)
	{
		if (config.ID == 0)
		{
			_log.Alert("[TaskExecutorManager] RemoveMazeBuff missing BuffID");
			return;
		}

		List<BaseEntity> resolvedTargets = ResolveTargets(config.TargetType, runtime);
		if (resolvedTargets.Count == 0)
		{
			_log.Alert("[TaskExecutorManager] RemoveMazeBuff failed: could not find valid targets");
			return;
		}

		bool broadcast = resolvedTargets.Count == 1;
		foreach (var target in resolvedTargets)
		{
			target.RemoveMazeBuff(config.ID, broadcast);
		}
	}

	private void ExecuteAddMazeBuff(AddMazeBuff config, TaskRuntime runtime)
	{
		if (config.ID == 0)
		{
			_log.Alert("[TaskExecutorManager] AddMazeBuff missing BuffID");
			return;
		}

		List<BaseEntity> resolvedTargets = ResolveTargets(config.TargetType, runtime);
		if (resolvedTargets.Count == 0)
		{
			_log.Alert("[TaskExecutorManager] AddMazeBuff failed: could not find valid targets");
			return;
		}

		bool broadcast = resolvedTargets.Count == 1;
		foreach (var target in resolvedTargets)
		{
			target.AddMazeBuff(config.ID, broadcast);
		}
	}
}