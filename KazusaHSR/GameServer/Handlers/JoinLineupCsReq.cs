using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleJoinLineupCsReq
{
	[Packet.PacketCmdId(PacketId.JoinLineupCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		JoinLineupCsReq req = packet.GetDecodedBody<JoinLineupCsReq>();
		JoinLineupScRsp rsp = new JoinLineupScRsp();
		if (req.ExtraLineupType == ExtraLineupType.LineupChallenge)
		{
			PlayerAvatar? virtualAvatar = session.player.avatarDict.Values
				.FirstOrDefault(a => a.AvatarId == req.AvatarId);
			if (virtualAvatar == null)
			{
				rsp.Retcode = (uint)Retcode.RetLineupAvatarNotExist;
				session.SendPacket(rsp);
				return;
			}
			var ret = session.player.ChallengeManager.SetVirtualAvatarInSlot((int)req.Slot, virtualAvatar);
			rsp.Retcode = (uint)ret;
			session.player.ChallengeManager.SendSyncLineupNotify();
			session.SendPacket(rsp);
			return;
		}
		int teamIndex = (int)req.Index;
		PlayerAvatar? avatar = session.player.avatarDict.Values
			.FirstOrDefault(a => a.AvatarId == req.AvatarId);
		if (avatar == null)
		{
			rsp.Retcode = (uint)Retcode.RetLineupAvatarNotExist;
			session.SendPacket(rsp);
			return;
		}
		if (session.player.TeamManager.GetTeamByIndex(teamIndex) == null)
		{
			rsp.Retcode = (uint)Retcode.RetLineupInvalidIndex;
			session.SendPacket(rsp);
			return;
		}
		if (session.player.TeamManager.GetTeamByIndex(teamIndex).Avatars.Any(a => a != null && a.AvatarId == req.AvatarId))
		{
			rsp.Retcode = (uint)Retcode.RetLineupAvatarAlreadyIn;
			session.SendPacket(rsp);
			return;
		}
		var result = session.player.TeamManager.SetAvatarInSlot(teamIndex, (int)req.Slot, avatar);
		rsp.Retcode = (uint)result;
		session.player.SendSyncLineupNotify();
		session.SendPacket(rsp);
	}
}