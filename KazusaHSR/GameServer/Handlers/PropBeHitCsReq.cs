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
			session.c.LogWarning($"Player {session.player.Uid} sent PropBeHitCsReq for non-prop entity {req.PropEntityId}");
			rsp.Retcode = (uint)Retcode.RetSceneUseSkillFail;
			session.SendPacket(rsp);
			return;
		}

		session.player.Scene.LevelGraphExecutor?.OnPropBeHit(prop);
		session.SendPacket(rsp);
	}
}
