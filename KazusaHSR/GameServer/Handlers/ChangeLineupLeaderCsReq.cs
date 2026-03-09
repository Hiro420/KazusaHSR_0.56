using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleChangeLineupLeaderCsReq
{
	[Packet.PacketCmdId(PacketId.ChangeLineupLeaderCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		ChangeLineupLeaderCsReq req = packet.GetDecodedBody<ChangeLineupLeaderCsReq>();
		ChangeLineupLeaderScRsp rsp = new ChangeLineupLeaderScRsp();
		var result = session.player.TeamManager.ChangeLeader((int)session.player.TeamIndex, (int)req.Slot);
		rsp.Retcode = (uint)result;
		if (result == Retcode.RetSucc)
		{
			rsp.Slot = req.Slot;
		}

		session.SendPacket(rsp);
	}
}