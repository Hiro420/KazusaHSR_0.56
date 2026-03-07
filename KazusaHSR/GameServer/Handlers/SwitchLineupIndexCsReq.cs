using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleSwitchLineupIndexCsReq
{
	[Packet.PacketCmdId(PacketId.SwitchLineupIndexCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		SwitchLineupIndexCsReq req = packet.GetDecodedBody<SwitchLineupIndexCsReq>();
		SwitchLineupIndexScRsp rsp = new SwitchLineupIndexScRsp()
		{
			Retcode = (uint)Retcode.RetSucc,
			Index = req.Index
		};
		if (req.Index == session.player.TeamIndex)
		{
			session.SendPacket(rsp);
			return;
		}
		if (req.Index >= session.player.TeamManager.TeamCount)
		{
			rsp.Retcode = (uint)Retcode.RetLineupInvalidIndex;
			session.SendPacket(rsp);
			return;
		}
		PlayerInfos.PlayerTeam OldTeam = session.player.TeamManager.GetTeamByIndex((int)session.player.TeamIndex);
		PlayerInfos.PlayerTeam NewTeam = session.player.TeamManager.GetTeamByIndex((int)req.Index);
		Retcode ret = session.player.TeamManager.SetActiveTeam(req.Index);
		// despawn all previous avatars and spawn new ones
		HashSet<uint> oldAvatarGuis = [];
		foreach (var avatar in OldTeam.Avatars.Where(a => a != null))
		{
			AvatarEntity? entity = session.player.FindEntityByPlayerAvatar(avatar);
			if (entity == null)
			{
				rsp.Retcode = (uint)Retcode.RetServerInternalError;
				session.SendPacket(rsp);
				return;
			}
			oldAvatarGuis.Add(entity._EntityId);
		}
		session.player.Scene?.EntityManager.DespawnMany(oldAvatarGuis);
		foreach (var avatar in NewTeam.Avatars.Where(a => a != null))
		{
			session.player.Scene!.SpawnAvatarEntity(avatar);
		}
		session.player.SendSyncLineupNotify();
		session.SendPacket(rsp);
	}
}