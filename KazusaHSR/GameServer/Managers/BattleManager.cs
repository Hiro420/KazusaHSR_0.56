using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public class BattleManager
{
	public Session session { get; private set; }
	public List<uint> CurrentStageIds { get; set; } = new List<uint>();
	public List<uint> MonsterEntityIds { get; set; } = new List<uint>();
	public List<uint> AssistEntityIds { get; set; } = new List<uint>();

	public BattleManager(Session session)
	{
		this.session = session;
	}

	public void StartCocoonBattle(uint stageId, uint propEntityId)
	{
		this.CurrentStageIds = new List<uint> { stageId };
		this.MonsterEntityIds = new List<uint>();
		this.AssistEntityIds = new List<uint>();
	}

	public void StartMonsterBattle(uint monsterEntityId, IEnumerable<uint>? assistEntityIds)
	{
		StartMonsterBattle(new[] { monsterEntityId }, assistEntityIds ?? Enumerable.Empty<uint>());
	}

	public void StartMonsterBattle(IEnumerable<uint>? monsterEntityIds, IEnumerable<uint>? assistEntityIds)
	{
		CurrentStageIds.Clear();
		MonsterEntityIds = (monsterEntityIds ?? Enumerable.Empty<uint>()).ToList();
		AssistEntityIds = (assistEntityIds ?? Enumerable.Empty<uint>()).ToList();

		foreach (var entityId in MonsterEntityIds.Concat(AssistEntityIds))
		{
			if (!session.player.Scene.EntityManager.TryGet(entityId, out BaseEntity bEntity))
				continue;

			if (bEntity is not MonsterEntity monsterEntity)
			{
				session.c.LogWarning($"Player {session.player.Uid} tried to start monster battle with non-monster entity {entityId}");
				continue;
			}

			uint eventId = monsterEntity.DbInfo.EventID;

			var planeEvent = MainApp.resourceManager.PlaneEventExcel
				.Where(e => e.EventID == eventId)
				.FirstOrDefault(i => i.WorldLevel == session.player.WorldLevel);

			if (planeEvent == null || planeEvent.StageID == 0)
			{
				session.c.LogWarning($"Player {session.player.Uid} tried to start monster battle but event ID {eventId} not found for monster entity {entityId}");
				continue;
			}

			CurrentStageIds.Add(planeEvent.StageID);
		}
	}

	public void ResetBattle()
	{
		this.CurrentStageIds = new List<uint>();
		this.MonsterEntityIds = new List<uint>();
		this.AssistEntityIds = new List<uint>();

		foreach (PlayerAvatar playerAvatar in session.player.GetCurrentLineup().Avatars.Where(a => a != null))
		{
			AvatarEntity? avatarEntity = session.player.Scene.EntityManager.TryGetByPlayerAvatar(playerAvatar);
			if (avatarEntity != null)
			{
				avatarEntity.RemoveAllMazeBuffs(true);
			}
		}
	}

	public Protocol.SceneBattleInfo GetCurrentBattleInfo()
	{
		Protocol.SceneBattleInfo battleInfo = new Protocol.SceneBattleInfo()
		{
			StageId = CurrentStageIds.First(),
			LogicRandomSeed = (uint)new Random().Next(),
			// todo: handle buffs by TaskConfig
		};
		foreach (uint stageId in CurrentStageIds)
		{
			StageRow stageRow = MainApp.resourceManager.StageExcel.First(s => s.StageID == stageId);
			battleInfo.MonsterWaveLists.AddRange(
				GetMonsterInfos(stageRow)
			);
		}
		foreach (PlayerAvatar avatarInfo in session.player.GetCurrentLineup().Avatars.Where(a => a != null))
		{
			battleInfo.BattleAvatarLists.Add(avatarInfo.ToBattleAvatar());

			AvatarEntity? avatarEntity = session.player.FindEntityByPlayerAvatar(avatarInfo);
			if (avatarEntity == null)
			{
				session.c.LogWarning($"Could not find AvatarEntity for PlayerAvatar {avatarInfo.AvatarId} when building BattleAvatar");
			}
			else
			{
				foreach (uint attachedBuffId in avatarEntity.AttachedMazeBuffs)
				{
					battleInfo.BuffLists.Add(new BattleBuff()
					{
						Id = attachedBuffId,
						Level = 1,
						// todo: figure out the rest
					});
				}
			}
		}

		return battleInfo;
	}

	private IEnumerable<SceneMonsterWave> GetMonsterInfos(StageRow stageRow)
	{
		List<SceneMonsterWave> monsterWaves = new();
		
		foreach (uint monsterId in stageRow.MonsterList.SelectMany(m => m.Values))
		{
			SceneMonsterWave monsterWave = new SceneMonsterWave()
			{
				MonsterIdLists = new uint[] { monsterId },
				// todo: drop lists
			};
			monsterWaves.Add(monsterWave);
		}

		return monsterWaves;
	}

	public bool IsInBattle()
	{
		return this.CurrentStageIds.Any();
	}

	public void OnBattleResult(PVEBattleResultCsReq req, out PVEBattleResultScRsp rsp)
	{
		rsp = new PVEBattleResultScRsp()
		{
			EndStatus = req.EndStatus,
			StageId = req.StageId,
		};

		switch (req.EndStatus)
		{
			case BattleEndStatus.BattleEndWin:
				// todo: calculate rewards and update player fight props
				List<uint> allEntityIds = new List<uint>()
					.Concat(MonsterEntityIds)
					.Concat(AssistEntityIds)
					.ToList();
				session.player.Scene.EntityManager.DespawnMany(allEntityIds);
				break;
			case BattleEndStatus.BattleEndQuit:
			case BattleEndStatus.BattleEndLose:
				// No rewards on failure
				this.session.player.ResetPosToLastEntrance();
				break;
			default:
				session.c.LogWarning($"Player {session.player.Uid} sent unknown battle end status {req.EndStatus}");
				rsp.Retcode = (uint)Retcode.RetBattleFail;
				break;
		}

		// Reset battle state
		ResetBattle();
	}
}
