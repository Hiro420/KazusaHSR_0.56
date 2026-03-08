using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleUnlockSkilltreeCsReq
{
	[Packet.PacketCmdId(PacketId.UnlockSkilltreeCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		UnlockSkilltreeCsReq req = packet.GetDecodedBody<UnlockSkilltreeCsReq>();
		UnlockSkilltreeScRsp rsp = new UnlockSkilltreeScRsp()
		{
			Level = req.Level,
			PointId = req.PointId,
			Retcode = (uint)Retcode.RetSucc,
		};
		
		// todo: deduct item, check if the player has enough items, etc.

		AvatarSkillTreeRow? row = MainApp.resourceManager.AvatarSkillTreeExcel.FirstOrDefault(r => r.PointID == req.PointId && r.Level == req.Level);
		if (row == null)
		{
			rsp.Retcode = (uint)Retcode.RetSkilltreeConfigNotExist;
			session.SendPacket(rsp);
			return;
		}
		PlayerAvatar? avatar = session.player.avatarDict.Values.FirstOrDefault(a => a.AvatarId == row.AvatarID);
		if (avatar == null)
		{
			rsp.Retcode = (uint)Retcode.RetAvatarNotExist;
			session.SendPacket(rsp);
			return;
		}
		if (!avatar.SkilltreeLists.ContainsKey(row.PointID))
		{
			rsp.Retcode = (uint)Retcode.RetSkilltreeConfigNotExist;
			session.SendPacket(rsp);
			return;
		}
		avatar.SkilltreeLists[row.PointID] = row.Level;
		rsp.AvatarId = avatar.AvatarId;

		PlayerSyncScNotify ntf = new PlayerSyncScNotify()
		{
			AvatarSync = new AvatarSync() { AvatarLists = { avatar.ToAvatarProto() } }
		};
		session.SendPacket(ntf);
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}