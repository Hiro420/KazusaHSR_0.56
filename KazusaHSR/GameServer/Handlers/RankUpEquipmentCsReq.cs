using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleRankUpEquipmentCsReq
{
	[Packet.PacketCmdId(PacketId.RankUpEquipmentCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		RankUpEquipmentCsReq req = packet.GetDecodedBody<RankUpEquipmentCsReq>();
		RankUpEquipmentScRsp rsp = new RankUpEquipmentScRsp();
		ItemLightcone? target = session.player.itemDict.Values.FirstOrDefault(x => x.Guid == req.EquipmentUniqueId && x is ItemLightcone) as ItemLightcone;
		IEnumerable<ItemLightcone> toconsume = [];
		HashSet<uint> idset = new HashSet<uint>();
		foreach (uint id in req.EquipmentIdLists)
		{
			ItemLightcone? item = session.player.itemDict.Values.FirstOrDefault(x => x.Guid == id && x is ItemLightcone) as ItemLightcone;
			if (item != null)
				toconsume = toconsume.Append(item);
			else
			{
				rsp.Retcode = (uint)Retcode.RetItemNotExist;
				session.SendPacket(rsp);
			}
		}
		if (target == null)
		{
			rsp.Retcode = (uint)Retcode.RetItemNotExist;
			session.SendPacket(rsp);
			return;
		}
		if (target.Rank >= 5)
		{
			rsp.Retcode = (uint)Retcode.RetEquipmentRankUpReachMax;
			session.SendPacket(rsp);
			return;
		}
		foreach (ItemLightcone item in toconsume)
		{
			idset.Add(item.Guid);
			session.player.itemDict.Remove(item.Guid);
			target.Rank += 1;
		}
		PlayerSyncScNotify ntf = new PlayerSyncScNotify()
		{
			EquipmentLists = { target.ToEquipmentProto() },
			DelEquipmentLists = idset.ToArray()
		};
		session.SendPacket(ntf);
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}