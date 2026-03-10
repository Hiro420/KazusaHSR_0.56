using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandlePromoteEquipmentCsReq
{
	[Packet.PacketCmdId(PacketId.PromoteEquipmentCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		PromoteEquipmentCsReq req = packet.GetDecodedBody<PromoteEquipmentCsReq>();
		PromoteEquipmentScRsp rsp = new PromoteEquipmentScRsp();

		// todo: handle item deduct. currently we just promote the equipment without checking cost or deducting items.
		PlayerItem? item = session.player.itemDict.GetValueOrDefault(req.EquipmentUniqueId);
		if (item == null)
		{
			rsp.Retcode = (uint)Retcode.RetItemNotExist;
			session.SendPacket(rsp);
			return;
		}
		if (item is not ItemLightcone lightcone)
		{
			rsp.Retcode = (uint)Retcode.RetItemInvalid;
			session.SendPacket(rsp);
			return;
		}
		lightcone.Promotion += 1;

		PlayerSyncScNotify ntf = new PlayerSyncScNotify()
		{
			EquipmentLists = { lightcone.ToEquipmentProto() }
		};

		session.SendPacket(ntf);
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}