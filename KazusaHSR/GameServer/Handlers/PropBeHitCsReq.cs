using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandlePropBeHitCsReq
{
	[Packet.PacketCmdId(PacketId.PropBeHitCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		// I still didnt receive this packet in testing, but hopefully this is all it does.
		PropBeHitCsReq req = packet.GetDecodedBody<PropBeHitCsReq>();
		PropBeHitScRsp rsp = new PropBeHitScRsp
		{
			PropEntityId = req.PropEntityId,
			Retcode = (uint)Retcode.RetSucc,
		};

		if (!session.player.Scene.EntityManager.TryGet(req.PropEntityId, out BaseEntity? entity) || entity is not PropEntity prop)
		{
			session.c.Alert($"Player {session.player.Uid} sent PropBeHitCsReq for non-prop entity {req.PropEntityId}");
			rsp.Retcode = (uint)Retcode.RetSceneUseSkillFail;
			session.SendPacket(rsp);
			return;
		}

		// Try to get the player entity (if exists)
		if (session.player.GetCurrentLineup() == null || session.player.GetCurrentLineup().Leader == null)
		{
			session.c.Alert($"Player {session.player.Uid} sent PropBeHitCsReq but has no avatar in the current lineup");
			rsp.Retcode = (uint)Retcode.RetSceneUseSkillFail;
			session.SendPacket(rsp);
			return;
		}
		AvatarEntity? spawnedAvatar = session.player.Scene.EntityManager.TryGetByPlayerAvatar(session.player.GetCurrentLineup().Leader!);

		session.player.Scene.TaskExecutor.OnPropBeHit(prop, spawnedAvatar);
		session.SendPacket(rsp);
	}
}
