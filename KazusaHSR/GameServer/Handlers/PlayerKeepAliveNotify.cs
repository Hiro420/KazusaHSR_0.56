using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandlePlayerKeepAliveNotify
{
	[Packet.PacketCmdId(PacketId.PlayerKeepAliveNotify)]
	public static void OnPacket(Session session, Packet packet)
	{
		// do nothing, its just heartbeat
	}
}