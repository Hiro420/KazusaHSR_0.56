using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetShopListCsReq
{
	[Packet.PacketCmdId(PacketId.GetShopListCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetShopListCsReq req = packet.GetDecodedBody<GetShopListCsReq>();
		GetShopListScRsp rsp = new GetShopListScRsp();
		// todo: actual logic
		session.SendPacket(rsp);
	}
}