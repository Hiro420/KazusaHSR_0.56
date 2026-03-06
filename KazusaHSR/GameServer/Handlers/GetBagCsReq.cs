using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetBagCsReq
{
	[Packet.PacketCmdId(PacketId.GetBagCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetBagCsReq req = packet.GetDecodedBody<GetBagCsReq>();
		GetBagScRsp rsp = new GetBagScRsp();
		foreach (PlayerItem equip in session.player.itemDict.Values)
		{
			rsp.MaterialLists.Add(equip.ToMaterialInfo());
		}
		// weapons later
		session.SendPacket(rsp);
	}
}