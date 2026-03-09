using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleBuyGoodsCsReq
{
	[Packet.PacketCmdId(PacketId.BuyGoodsCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		BuyGoodsCsReq req = packet.GetDecodedBody<BuyGoodsCsReq>();
		BuyGoodsScRsp rsp = new BuyGoodsScRsp()
		{
			ShopId = req.ShopId,
			GoodsId = req.GoodsId,
		};
		rsp.Retcode = (uint)session.player.ShopManager.BuyGood(req.ShopId, req.GoodsId, req.GoodsNum);
		rsp.GoodsBuyTimes = session.player.ShopManager.GetGoodsBuyTimes(req.ShopId, req.GoodsId);
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}