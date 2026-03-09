using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetBagCsReq
{
	[Packet.PacketCmdId(PacketId.GetBagCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetBagCsReq req = packet.GetDecodedBody<GetBagCsReq>();
		GetBagScRsp rsp = new GetBagScRsp();
		foreach (PlayerItem equip in session.player.ItemManager.Items)
		{
			equip.AllToRsp(rsp);
		}
		// weapons later
		session.SendPacket(rsp);
	}
}