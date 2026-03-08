using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleLeaveChallengeCsReq
{
	[Packet.PacketCmdId(PacketId.LeaveChallengeCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		LeaveChallengeCsReq req = packet.GetDecodedBody<LeaveChallengeCsReq>();
		LeaveChallengeScRsp rsp = new LeaveChallengeScRsp();
		Maze? maze;
		var ret = session.player!.ChallengeManager.LeaveChallenge(out maze);
		rsp.Retcode = (uint)ret;
		if (ret == Retcode.RetSucc && maze != null)
		{
			rsp.Maze = maze;
		}
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}
