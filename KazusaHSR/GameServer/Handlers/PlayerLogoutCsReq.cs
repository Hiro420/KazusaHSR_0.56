using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandlePlayerLogoutCsReq
{
	[Packet.PacketCmdId(PacketId.PlayerLogoutCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		PlayerLogoutCsReq req = packet.GetDecodedBody<PlayerLogoutCsReq>();
		if (session.player == null)
		{
			session.c.LogWarning("Received PlayerLogoutCsReq for unauthenticated session.");
			return;
		}
		session.c.LogWarning($"Player {session.player!.Uid} requested logout.");
		session.Terminate();
	}
}