using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleSwapLineupCsReq
{
	[Packet.PacketCmdId(PacketId.SwapLineupCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		SwapLineupCsReq req = packet.GetDecodedBody<SwapLineupCsReq>();
		SwapLineupScRsp rsp = new SwapLineupScRsp();

		// Challenge virtual lineup swap
		if (req.ExtraLineupType == ExtraLineupType.LineupChallenge)
		{
			var ret = session.player.ChallengeManager.SwapVirtualSlots((int)req.SrcSlot, (int)req.DstSlot);
			rsp.Retcode = (uint)ret;
			if (ret == Retcode.RetSucc)
			{
				session.player.ChallengeManager.SendSyncLineupNotify();
			}
			session.SendPacket(rsp);
			return;
		}

		// Normal team lineup swap
		int teamIndex = (int)req.Index;
		var result = session.player.TeamManager.SwapAvatars(teamIndex, (int)req.SrcSlot, (int)req.DstSlot);
		rsp.Retcode = (uint)result;
		if (result == Retcode.RetSucc)
		{
			session.player.SendSyncLineupNotify();
		}

		session.SendPacket(rsp);
	}
}