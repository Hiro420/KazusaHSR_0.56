using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleSceneCastSkillCsReq
{
	[Packet.PacketCmdId(PacketId.SceneCastSkillCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		SceneCastSkillCsReq req = packet.GetDecodedBody<SceneCastSkillCsReq>();
		SceneCastSkillScRsp rsp = new SceneCastSkillScRsp();

		// todo: handle skillIndex effects, apply damage/buffs/debuffs, etc using TaskConfig
		if (session.player.battleManager.IsInBattle())
		{
			session.c.LogWarning($"Player {session.player.Uid} tried to cast skill during battle, wtf?");
			rsp.Retcode = (uint)Retcode.RetSceneUseSkillFail;
			session.SendPacket(rsp);
			return;
		}

		// todo: handle AbilityTargetEntityId
		if (!session.player.Scene.EntityManager.TryGet(req.CastEntityId, out BaseEntity? iEntity))
		{
			session.c.LogWarning($"Player {session.player.Uid} tried to cast skill with non-existing entity {req.CastEntityId}");
			rsp.Retcode = (uint)Retcode.RetSceneUseSkillFail;
			session.SendPacket(rsp);
			return;
		}

		if (iEntity is MonsterEntity monsterEntity)
		{
			session.player.battleManager.StartMonsterBattle(
				monsterEntity._EntityId,
				req.AssistMonsterEntityIdLists
			);
			rsp.BattleInfo = session.player.battleManager.GetCurrentBattleInfo();
		}
		else if (iEntity is AvatarEntity avatarEntity)
		{
			var entities = session.player.Scene.EntityManager.Entities;

			// todo: target the AbilityTargetEntityId in ability manager
			// for now just pass all hit target entities
			session.AbilityManager.OnAvatarMazeSkill(avatarEntity.DbInfo, req, rsp);
		}

		session.SendPacket(rsp);
	}
}