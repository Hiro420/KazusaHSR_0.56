using KazusaHSR.Protocol;

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
			IsVirtual = req.IsVirtual
		};
		if (req.ExtraLineupType == ExtraLineupType.LineupChallenge)
		{
			var ret = session.player.ChallengeManager.RemoveVirtualAvatar(req.AvatarId);
			if (ret != Retcode.RetSucc)
			{
				rsp.Retcode = (uint)ret;
			}
			rsp.IsVirtual = false;
			rsp.IsMainline = false;
			rsp.PlaneId = session.player.ChallengeManager.VirtualLineup.PlaneId;
			session.player.ChallengeManager.SendSyncLineupNotify();
			session.SendPacket(rsp);
			return;
		}
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