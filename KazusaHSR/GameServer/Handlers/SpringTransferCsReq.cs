using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleSpringTransferCsReq
{
	[Packet.PacketCmdId(PacketId.SpringTransferCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		SpringTransferCsReq req = packet.GetDecodedBody<SpringTransferCsReq>();
		SpringTransferScRsp rsp = new SpringTransferScRsp();

		if (!session.player!.Scene.EntityManager.TryGet(req.PropEntityId, out BaseEntity? springTransferEntity))
		{
			session.c.LogError($"Player {session.player!.Uid} tried to use invalid spring transfer prop entity ID {req.PropEntityId}.");
			rsp.Retcode = (uint)Retcode.RetMazePropNotExist;
			session.SendPacket(rsp);
			return;
		}
		if (springTransferEntity is not PropEntity springTransferProp)
		{
			session.c.LogError($"Player {session.player!.Uid} tried to use non-spring transfer prop entity ID {req.PropEntityId}.");
			rsp.Retcode = (uint)Retcode.RetMazePropNotExist;
			session.SendPacket(rsp);
			return;
		}

		session.player.SetPos(springTransferEntity.Position);
		session.player.SetRot(springTransferEntity.Rotation);

		if (!session.player.Scene.EntityManager.TryGet(session.player.GetLeaderEntityId(), out BaseEntity? leaderEntity))
		{
			rsp.Retcode = (uint)Retcode.RetServerInternalError;
			session.SendPacket(rsp);
			return;
		}

		leaderEntity.Position = session.player.Pos;
		leaderEntity.Rotation = session.player.Rot;

		// idk if i should, but ill send it anyway
		session.SendPacket(new SceneEntityMoveScNotify()
		{
			EntityId = session.player.GetLeaderEntityId(),
			Motion = leaderEntity.GetMotionInfo(),
		});

		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}