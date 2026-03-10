using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public sealed class AdventureAbilityContext
{
	public Session Session { get; }
	public PlayerAvatar Avatar { get; }
	public AdventureAbilityConfig AbilityConfig { get; }
	public SceneCastSkillCsReq Request { get; }
	public SceneCastSkillScRsp Response { get; }

	public AdventureAbilityContext(Session session, PlayerAvatar avatar, AdventureAbilityConfig abilityConfig, SceneCastSkillCsReq request, SceneCastSkillScRsp response)
	{
		Session = session;
		Avatar = avatar;
		AbilityConfig = abilityConfig;
		Request = request;
		Response = response;
	}

	public BaseEntity? GetAbilityEntity()
	{
		if (Request.AbilityTargetEntityId == 0)
			return null;
		Session.player.Scene.EntityManager.TryGet(Request.AbilityTargetEntityId, out BaseEntity? entity);
		return entity;
	}
}

// OH OH THIS IS THE GOOD STUFF
public sealed class AdventureTaskExecutor
{
	private readonly AdventureAbilityContext _ctx;

	public AdventureTaskExecutor(AdventureAbilityContext ctx)
	{
		_ctx = ctx;
	}

	public void ExecuteTasks(IEnumerable<TaskConfig> tasks)
	{
		if (tasks == null)
			return;

		foreach (var task in tasks)
		{
			ExecuteTask(task);
		}
	}

	private void ExecuteTask(TaskConfig? task)
	{
		if (task == null) //  || !task.TaskEnabled
			return;

		switch (task)
		{
			case PredicateTaskList predicateList:
				ExecutePredicateTaskList(predicateList);
				break;

			case AdventureTriggerAttack triggerAttack:
				ExecuteAdventureTriggerAttack(triggerAttack);
				break;

			case AdventureModifyTeamPlayerHP modifyHp:
				ExecuteAdventureModifyTeamPlayerHP(modifyHp);
				break;

			case AdventureFireProjectile fireProjectile:
				ExecuteAdventureFireProjectile(fireProjectile);
				break;

			case AddMazeBuff addMazeBuff:
				ExecuteAddMazeBuff(addMazeBuff, _ctx.Request.AbilityTargetEntityId);
				break;

			case RemoveMazeBuff removeMazeBuff:
				ExecuteRemoveMazeBuff(removeMazeBuff, _ctx.Request.AbilityTargetEntityId);
				break;

			default:
				_ctx.Session.c.LogInfo($"[AdventureTaskExecutor] Unhandled task type: {task.GetType().FullName}");
				break;
		}
	}

	private void ExecutePredicateTaskList(PredicateTaskList config)
	{
		bool result = EvaluatePredicate(config.Predicate);
		_ctx.Session.c.LogInfo($"[AdventureTaskExecutor] PredicateTaskList => {(result ? "Success" : "Failed")}");

		var list = result ? config.SuccessTaskList : config.FailedTaskList;
		if (list != null)
		{
			ExecuteTasks(list);
		}
	}

	private bool EvaluatePredicate(PredicateConfig? predicate)
	{
		if (predicate == null)
			return true;

		// TODO: add more
		switch (predicate)
		{
			case ByHaveAbilityTarget:
				return _ctx.GetAbilityEntity() != null;

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
			case ByIsContainAdventureModifier byIsContainAdventureModifier:
				{
					_ctx.Session.c.LogInfo($"[AdventureTaskExecutor] Evaluating ByIsContainAdventureModifier for ModifierId={byIsContainAdventureModifier.ModifierName}");
					IEnumerable<BaseEntity> targetEntities = EvaluateTargets(byIsContainAdventureModifier.TargetType);
					foreach (var entity in targetEntities)
					{
						bool hasModifier = entity.HasAdventureModifier(byIsContainAdventureModifier.ModifierName);
						_ctx.Session.c.LogInfo($"[AdventureTaskExecutor] Entity {entity._EntityId} has modifier {byIsContainAdventureModifier.ModifierName}: {hasModifier}");
						return hasModifier;
					}
					return false;
				}
			default:
				_ctx.Session.c.LogInfo($"[AdventureTaskExecutor] Unhandled predicate type: {predicate.GetType().FullName}, default=true");
				return true;
		}
	}

	// Not really dummy, just without an ability config
	public void CreateDummyFight()
	{
		ExecuteAdventureTriggerAttack(new AdventureTriggerAttack
		{
			OnAttack = []
		});
	}

	private void ExecuteAdventureTriggerAttack(AdventureTriggerAttack config)
	{
		var session = _ctx.Session;
		var req = _ctx.Request;
		var rsp = _ctx.Response;
		var entities = session.player.Scene.EntityManager.Entities;

		_ctx.Session.c.LogInfo("[AdventureTaskExecutor] AdventureTriggerAttack invoked");

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
			ExecuteTasks(config.OnAttack);
			session.player.battleManager.StartMonsterBattle(hitMonsterIds, assistMonsterIds);
			rsp.BattleInfo = session.player.battleManager.GetCurrentBattleInfo();
		}
		foreach (uint propGuid in hitPropIds)
		{
			PropEntity? propEntity = entities[propGuid] as PropEntity;
			if (propEntity == null)
			{
				session.c.LogWarning($"Prop {propGuid} is not a valid PropEntity? WTF?");
				continue;
			}
			session.player.Scene.LevelGraphExecutor?.OnPropBeHit(propEntity);
		}
	}

	private void ExecuteAdventureModifyTeamPlayerHP(AdventureModifyTeamPlayerHP config)
	{
		_ctx.Session.c.LogInfo("[AdventureTaskExecutor] AdventureModifyTeamPlayerHP invoked (stub)");
	}

	private void ExecuteAdventureFireProjectile(AdventureFireProjectile config)
	{
		_ctx.Session.c.LogInfo("[AdventureTaskExecutor] AdventureFireProjectile invoked");
		bool isHit = _ctx.Request.HitTargetEntityIdLists != null &&
			_ctx.Request.HitTargetEntityIdLists.Length > 0;
		if (isHit)
		{
			_ctx.Session.c.LogInfo("[AdventureTaskExecutor] Projectile hit detected, executing OnProjectileHit tasks");
			if (config.OnProjectileHit != null)
				ExecuteTasks(config.OnProjectileHit);
			else
				ExecuteTasks(config.OnProjectileLifetimeFinish);
		}
	}

	private IEnumerable<BaseEntity> EvaluateTargets(TargetEvaluator? targetType)
	{
		List<BaseEntity> targetEntities =
		[
			_ctx.Session.player.FindEntityByPlayerAvatar(_ctx.Avatar)!
		];

		if (targetType is not TargetAlias alias)
			return targetEntities;

		switch (alias.Alias)
		{
			case "LightTeamEntity":
				targetEntities.Clear();

				IEnumerable<PlayerAvatar> teamAvatars = _ctx.Session.player.ChallengeManager.IsInChallenge ?
					_ctx.Session.player.ChallengeManager.VirtualLineup.AvatarIds.Where(id => id != 0).Select(id => _ctx.Session.player.avatarDict.Values.First(a => a.AvatarId == id))
					: _ctx.Session.player.GetCurrentLineup().Avatars;
				foreach (var avatar in teamAvatars)
				{
					BaseEntity? entity = _ctx.Session.player.FindEntityByPlayerAvatar(avatar);
					if (entity != null)
						targetEntities.Add(entity);
				}
				break;
		}

		return targetEntities;
	}

	private void ExecuteAddMazeBuff(AddMazeBuff config, uint targetEntityId)
	{
		double duration = config.LifeTime != null ? config.LifeTime.Evaluate() : -1;
		_ctx.Session.c.LogInfo($"[AdventureTaskExecutor] AddMazeBuff invoked: BuffId={config.ID}, Duration={duration}s");

		// todo: handle duration
		IEnumerable<BaseEntity> targetEntities = EvaluateTargets(config.TargetType);
		//AvatarEntity? avatarEntity = _ctx.Session.player.Scene.EntityManager.TryGetByPlayerAvatar(_ctx.Avatar);
		//if (avatarEntity != null)
		//{
		//	avatarEntity.AddMazeBuff(config.ID);
		//}
		//else
		//{
		//	_ctx.Session.c.LogWarning($"[AdventureTaskExecutor] AddMazeBuff: AvatarEntity not found for AvatarId={_ctx.Avatar.AvatarId}");
		//}
		foreach (var entity in targetEntities)
		{
			entity.AddMazeBuff(config.ID);
			_ctx.Session.c.LogInfo($"[AdventureTaskExecutor] Added MazeBuff {config.ID} to Entity {entity._EntityId}");
		}
	}

	private void ExecuteRemoveMazeBuff(RemoveMazeBuff config, uint targetEntityId)
	{
		_ctx.Session.c.LogInfo($"[AdventureTaskExecutor] RemoveMazeBuff invoked: BuffId={config.ID}");
		IEnumerable<BaseEntity> targetEntities = EvaluateTargets(config.TargetType);
		foreach (var entity in targetEntities)
		{
			entity.RemoveMazeBuff(config.ID);
			_ctx.Session.c.LogInfo($"[AdventureTaskExecutor] Removed MazeBuff {config.ID} from Entity {entity._EntityId}");
		}
	}
}
