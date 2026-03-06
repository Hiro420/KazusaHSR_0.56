using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetStaminaExchangeCsReq
{
	[Packet.PacketCmdId(PacketId.GetStaminaExchangeCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetStaminaExchangeCsReq req = packet.GetDecodedBody<GetStaminaExchangeCsReq>();
		GetStaminaExchangeScRsp rsp = new GetStaminaExchangeScRsp();
		// todo: actual logic
		session.SendPacket(rsp);
	}
}