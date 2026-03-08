using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleExpUpEquipmentCsReq
{
	[Packet.PacketCmdId(PacketId.ExpUpEquipmentCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		ExpUpEquipmentCsReq req = packet.GetDecodedBody<ExpUpEquipmentCsReq>();
		ExpUpEquipmentScRsp rsp = new ExpUpEquipmentScRsp();
		//ItemLightcone? target = session.player.itemDict.Values.FirstOrDefault(x => x.Guid == req.EquipmentUniqueId && x is ItemLightcone) as ItemLightcone;
		// todo: implement
		rsp.Retcode = (uint)Retcode.RetServerInternalError;
		session.SendPacket(rsp);
	}
}