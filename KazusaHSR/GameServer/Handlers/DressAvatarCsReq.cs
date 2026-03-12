using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleDressAvatarCsReq
{
	[Packet.PacketCmdId(PacketId.DressAvatarCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		DressAvatarCsReq req = packet.GetDecodedBody<DressAvatarCsReq>();
		DressAvatarScRsp rsp = new DressAvatarScRsp();
		PlayerAvatar? avatar = session.player.avatarDict.Values.FirstOrDefault(i => i.AvatarId == req.AvatarId);
		PlayerItem? item = session.player.itemDict.FirstOrDefault(i => i.Key == req.EquipmentUniqueId).Value;
		PlayerSyncScNotify notify = new PlayerSyncScNotify()
		{
			AvatarSync = new AvatarSync(),
		};
		if (avatar == null)
		{
			session.c.Fail($"[DressAvatar] Could not find avatar with ID {req.AvatarId}");
			rsp.Retcode = (uint)Retcode.RetAvatarNotExist;
			session.SendPacket(rsp);
			return;
		}
		if (item == null)
		{
			session.c.Fail($"[DressAvatar] Could not find item with unique ID {req.EquipmentUniqueId}");
			rsp.Retcode = (uint)Retcode.RetItemNotExist;
			session.SendPacket(rsp);
			return;
		}
		if (item is not ItemLightcone lightcone)
		{
			session.c.Fail($"[DressAvatar] Item with unique ID {req.EquipmentUniqueId} is not a lightcone");
			rsp.Retcode = (uint)Retcode.RetAvatarDressNoEquipment;
			session.SendPacket(rsp);
			return;
		}
		ItemLightcone? prevLightcone = null;
		if (avatar.EquipGuid != 0)
		{
			prevLightcone = session.player.itemDict.Values.FirstOrDefault(i => i is ItemLightcone && i.Guid == avatar.EquipGuid) as ItemLightcone;
		}
		if (lightcone.BelongAvatarId != 0 && lightcone.BelongAvatarId != avatar.AvatarId)
		{
			PlayerAvatar avatar1 = session.player.avatarDict.Values.First(i => i.AvatarId == lightcone.BelongAvatarId);
			avatar1.EquipGuid = 0;
			notify.AvatarSync.AvatarLists.Add(avatar1.ToAvatarProto());
		}
		session.player.EquipLightcone(avatar, lightcone);
		notify.AvatarSync.AvatarLists.Add(avatar.ToAvatarProto());
		notify.EquipmentLists.Add(lightcone.ToEquipmentProto());
		if (prevLightcone != null)
		{
			prevLightcone.BelongAvatarId = 0;
			notify.EquipmentLists.Add(prevLightcone.ToEquipmentProto());
		}
		session.SendPacket(notify);
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}