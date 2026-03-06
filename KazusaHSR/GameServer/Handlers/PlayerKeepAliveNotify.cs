using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandlePlayerKeepAliveNotify
{
	[Packet.PacketCmdId(PacketId.PlayerKeepAliveNotify)]
	public static void OnPacket(Session session, Packet packet)
	{
		// do nothing, its just heartbeat
	}
}