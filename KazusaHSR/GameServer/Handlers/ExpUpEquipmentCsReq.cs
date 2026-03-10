using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleExpUpEquipmentCsReq
{
	[Packet.PacketCmdId(PacketId.ExpUpEquipmentCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		ExpUpEquipmentCsReq req = packet.GetDecodedBody<ExpUpEquipmentCsReq>();
		ExpUpEquipmentScRsp rsp = new ExpUpEquipmentScRsp();

		ItemLightcone? target = session.player.itemDict.Values.FirstOrDefault(x => x.Guid == req.EquipmentUniqueId && x is ItemLightcone) as ItemLightcone;
		if (target == null)
		{
			rsp.Retcode = (uint)Retcode.RetItemNotExist;
			session.SendPacket(rsp);
			return;
		}
		rsp.Retcode = (uint)target.AddExp(req.CostData.ItemLists);
		PlayerSyncScNotify ntf = new PlayerSyncScNotify()
		{
			EquipmentLists = { target.ToEquipmentProto() },
		};
		session.SendPacket(ntf);
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}