using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleFinishChallengeCsReq
{
	[Packet.PacketCmdId(PacketId.FinishChallengeCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		FinishChallengeCsReq req = packet.GetDecodedBody<FinishChallengeCsReq>();
		FinishChallengeScRsp rsp = new FinishChallengeScRsp();
		var ret = session.player!.ChallengeManager.FinishChallenge(req.ChallengeId, out ChallengeSettleNotify notify);
		rsp.Retcode = (uint)ret;
		if (ret == Retcode.RetSucc)
		{
			session.SendPacket(notify);
		}
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}
