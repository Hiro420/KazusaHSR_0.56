using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleLockEquipmentCsReq
{
	[Packet.PacketCmdId(PacketId.LockEquipmentCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		LockEquipmentCsReq req = packet.GetDecodedBody<LockEquipmentCsReq>();
		LockEquipmentScRsp rsp = new LockEquipmentScRsp()
		{
			EquipmentUniqueId = req.EquipmentUniqueId
		};

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
		lightcone.IsProtected = req.IsLock;
		session.SendPacket(rsp);
	}
}