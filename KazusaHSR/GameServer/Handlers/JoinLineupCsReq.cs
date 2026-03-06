using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleJoinLineupCsReq
{
	[Packet.PacketCmdId(PacketId.JoinLineupCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		JoinLineupCsReq req = packet.GetDecodedBody<JoinLineupCsReq>();
		JoinLineupScRsp rsp = new JoinLineupScRsp();

		// todo: handle virtual avatars and extra lineup types
		PlayerTeam playerTeam = session.player.teamList[(int)req.Index-1];
		PlayerAvatar? avatar = session.player.avatarDict.Values
			.FirstOrDefault(a => a.AvatarId == req.AvatarId);
		if (avatar == null)
		{
			rsp.Retcode = (uint)Retcode.RetLineupAvatarNotExist;
			session.SendPacket(rsp);
			return;
		}
		rsp.Retcode = (uint)playerTeam.SqueezeAvatarIn(session, avatar, (int)req.Slot);
		session.player.SendSyncLineupNotify();
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}