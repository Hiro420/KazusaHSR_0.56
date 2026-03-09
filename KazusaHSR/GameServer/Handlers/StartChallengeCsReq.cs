using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleStartChallengeCsReq
{
	[Packet.PacketCmdId(PacketId.StartChallengeCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		StartChallengeCsReq req = packet.GetDecodedBody<StartChallengeCsReq>();
		StartChallengeScRsp rsp = new StartChallengeScRsp()
		{
			Retcode = (uint)session.player.ChallengeManager.EnterChallenge(req.ChallengeId)
		};
		rsp.Maze = session.player.ChallengeManager.CurMaze;
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}