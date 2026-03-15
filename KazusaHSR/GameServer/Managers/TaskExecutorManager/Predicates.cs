using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;

namespace KazusaHSR.GameServer;

public sealed partial class TaskExecutorManager
{
	private void ExecutePredicateTaskList(PredicateTaskList config, TaskRuntime runtime)
	{
		bool result = EvaluatePredicate(config.Predicate, runtime);
		var list = result ? config.SuccessTaskList : config.FailedTaskList;
		if (list != null)
		{
			ExecuteTaskList(list.Where(t => t != null).ToList(), runtime);
		}
	}

	private bool EvaluatePredicate(PredicateConfig? predicate, TaskRuntime runtime)
	{
		if (predicate == null)
			return runtime.Kind == RuntimeKind.Adventure;

		switch (predicate)
		{
			case ByHaveAbilityTarget:
				return runtime.AdventureContext?.GetAbilityEntity() != null;

			case ByCompareGroupMonsterNum byMonster:
				{
					uint groupId = byMonster.GroupID != 0 ? byMonster.GroupID : runtime.GroupId;
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
						if (!EvaluatePredicate(child, runtime))
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
						if (EvaluatePredicate(child, runtime))
							return true;
					}
					return false;
				}

			case ByIsContainAdventureModifier byIsContainAdventureModifier:
				return ResolveTargets(byIsContainAdventureModifier.TargetType, runtime)
					.Any(entity => entity.HasAdventureModifier(byIsContainAdventureModifier.ModifierName));

			case ByTargetTeam byTargetTeam:
				return ResolveTargets(byTargetTeam.TargetType, runtime)
					.Any(entity => IsEntityInTeam(entity, byTargetTeam.Team));

			default:
				_log.Message($"[TaskExecutorManager] EvaluatePredicate: unsupported predicate type {predicate.GetType().FullName}");
				return runtime.Kind == RuntimeKind.Adventure;
		}
	}

	private static bool IsEntityInTeam(BaseEntity entity, TeamType team)
	{
		return team switch
		{
			TeamType.TeamLight => entity is AvatarEntity,
			TeamType.TeamDark => entity is MonsterEntity,
			TeamType.TeamNPC => entity is NpcEntity,
			TeamType.TeamNeutral => entity is PropEntity,
			_ => false,
		};
	}

	private List<BaseEntity> ResolveTargets(TargetEvaluator? targetType, TaskRuntime runtime)
	{
		if (targetType is not TargetAlias targetAlias)
		{
			return ResolveLightTeamEntities();
		}

		switch (targetAlias.Alias)
		{
			case "TaskActionTarget":
				if (runtime.OwnerPropEntityId != 0 && _scene.EntityManager.TryGet(runtime.OwnerPropEntityId, out BaseEntity? taskTarget) && taskTarget != null)
				{
					return new List<BaseEntity> { taskTarget };
				}
				return new List<BaseEntity>();

			case "AbilityTarget":
				if (runtime.AdventureContext?.GetAbilityEntity() is BaseEntity abilityTarget)
				{
					return new List<BaseEntity> { abilityTarget };
				}
				return new List<BaseEntity>();

			case "LineupLeader":
				{
					var leaderAvatar = _session.player.GetCurrentLineup().Leader;
					if (leaderAvatar == null)
						return new List<BaseEntity>();

					var leader = _session.player.FindEntityByPlayerAvatar(leaderAvatar);
					if (leader == null)
						return new List<BaseEntity>();

					return new List<BaseEntity> { leader };
				}

			case "LightTeamEntity":
				return ResolveLightTeamEntities();

			default:
				_log.Alert($"[TaskExecutorManager] Unknown TargetAlias: {targetAlias.Alias}, fallback to LineupLeader");
				goto case "LineupLeader";
		}
	}

	private List<BaseEntity> ResolveLightTeamEntities()
	{
		IEnumerable<PlayerAvatar> teamAvatars = _session.player.ChallengeManager.IsInChallenge
			? _session.player.ChallengeManager.VirtualLineup.AvatarIds
				.Where(id => id != 0)
				.Select(id => _session.player.avatarDict.Values.FirstOrDefault(a => a.AvatarId == id))
				.Where(a => a != null)
				.Cast<PlayerAvatar>()
			: _session.player.GetCurrentLineup().Avatars;

		return teamAvatars
			.Select(_session.player.FindEntityByPlayerAvatar)
			.Where(e => e != null)
			.Cast<BaseEntity>()
			.ToList();
	}
}