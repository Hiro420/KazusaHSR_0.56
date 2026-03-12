using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandlePlayerLogoutCsReq
{
	[Packet.PacketCmdId(PacketId.PlayerLogoutCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		PlayerLogoutCsReq req = packet.GetDecodedBody<PlayerLogoutCsReq>();
		if (session.player == null)
		{
			session.c.Alert("Received PlayerLogoutCsReq for unauthenticated session.");
			return;
		}
		session.c.Alert($"Player {session.player!.Uid} requested logout.");
		session.Terminate();
	}
}