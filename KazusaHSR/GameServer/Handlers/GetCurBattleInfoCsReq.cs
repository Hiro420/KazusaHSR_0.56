using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetCurBattleInfoCsReq
{
	[Packet.PacketCmdId(PacketId.GetCurBattleInfoCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetCurBattleInfoCsReq req = packet.GetDecodedBody<GetCurBattleInfoCsReq>();
		GetCurBattleInfoScRsp rsp = new GetCurBattleInfoScRsp();
		// todo: implement battle info retrieval
		session.SendPacket(rsp);
	}
}