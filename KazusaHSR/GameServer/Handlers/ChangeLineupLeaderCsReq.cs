using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleChangeLineupLeaderCsReq
{
	[Packet.PacketCmdId(PacketId.ChangeLineupLeaderCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		ChangeLineupLeaderCsReq req = packet.GetDecodedBody<ChangeLineupLeaderCsReq>();
		ChangeLineupLeaderScRsp rsp = new ChangeLineupLeaderScRsp();
		PlayerTeam playerTeam = session.player.GetCurrentLineup();

		if (req.Slot >= playerTeam.Avatars.Count)
		{
			rsp.Retcode = (int)Retcode.RetLineupAvatarNotExist; // invalid slot
		}
		else
		{
			session.player.GetCurrentLineup().SetLeader(session, playerTeam.Avatars[(int)req.Slot]);
			rsp.Slot = req.Slot;
		}

		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}