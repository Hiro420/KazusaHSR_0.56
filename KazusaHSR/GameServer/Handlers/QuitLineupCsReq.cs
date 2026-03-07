using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleQuitLineupCsReq
{
	[Packet.PacketCmdId(PacketId.QuitLineupCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		QuitLineupCsReq req = packet.GetDecodedBody<QuitLineupCsReq>();
		QuitLineupScRsp rsp = new QuitLineupScRsp()
		{
			Retcode = (uint)Retcode.RetSucc,
			AvatarId = req.AvatarId,
			IsVirtual = req.IsVirtual,
		};
		if (req.Index >= session.player.TeamManager.TeamCount)
		{
			rsp.Retcode = (uint)Retcode.RetLineupInvalidIndex;
			session.SendPacket(rsp);
			return;
		}
		PlayerInfos.PlayerTeam team = session.player.TeamManager.GetTeamByIndex((int)req.Index);
		PlayerInfos.PlayerAvatar? avatar = team.Avatars.FirstOrDefault(a => a != null && a.AvatarId == req.AvatarId);
		if (avatar == null)
		{
			rsp.Retcode = (uint)Retcode.RetLineupAvatarNotExist;
			session.SendPacket(rsp);
			return;
			//session.player.Scene!.DespawnAvatarEntity(avatar);
		}
		team.RemoveAvatar(session, avatar);
		if (req.Index == session.player.TeamIndex)
		{
			session.player.Scene!.DespawnAvatarEntity(avatar);
			session.player.SendSyncLineupNotify();
		}
		session.SendPacket(rsp);
	}
}