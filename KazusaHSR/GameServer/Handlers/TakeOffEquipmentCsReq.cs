using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleTakeOffEquipmentCsReq
{
	[Packet.PacketCmdId(PacketId.TakeOffEquipmentCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		TakeOffEquipmentCsReq req = packet.GetDecodedBody<TakeOffEquipmentCsReq>();
		TakeOffEquipmentScRsp rsp = new TakeOffEquipmentScRsp();
		PlayerAvatar? playerAvatar = session.player.avatarDict.Values.FirstOrDefault(x => x.AvatarId == req.AvatarId);
		if (playerAvatar == null)
		{
			rsp.Retcode = (uint)Retcode.RetAvatarNotExist;
			session.SendPacket(rsp);
			return;
		}
		if (playerAvatar.EquipGuid == 0)
		{
			rsp.Retcode = (uint)Retcode.RetAvatarDressNoEquipment;
			session.SendPacket(rsp);
			return;
		}
		ItemLightcone? lightcone = session.player.itemDict.Values.FirstOrDefault(x => x is ItemLightcone && x.Guid == playerAvatar.EquipGuid) as ItemLightcone;
		if (lightcone == null)
		{
			rsp.Retcode = (uint)Retcode.RetItemNotExist;
			session.SendPacket(rsp);
			return;
		}
		playerAvatar.EquipGuid = 0;
		lightcone.BelongAvatarId = 0;
		PlayerSyncScNotify ntf = new PlayerSyncScNotify()
		{
			AvatarSync = new AvatarSync()
			{
				AvatarLists = { playerAvatar.ToAvatarProto() }
			},
			EquipmentLists = { lightcone.ToEquipmentProto() }
		};
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}