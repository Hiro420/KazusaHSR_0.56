using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetShopListCsReq
{
	[Packet.PacketCmdId(PacketId.GetShopListCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetShopListCsReq req = packet.GetDecodedBody<GetShopListCsReq>();
		GetShopListScRsp rsp = new GetShopListScRsp()
		{
			ShopType = req.ShopType,
		};
		foreach (var shop in session.player.ShopManager.GetAllShops(req.ShopType))
		{
			rsp.ShopLists.Add(shop);
		}
		session.SendPacket(rsp);
	}
}