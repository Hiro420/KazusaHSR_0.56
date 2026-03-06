using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandlePVEBattleResultCsReq
{
	[Packet.PacketCmdId(PacketId.PVEBattleResultCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		PVEBattleResultCsReq req = packet.GetDecodedBody<PVEBattleResultCsReq>();
		if (!session.player.battleManager.IsInBattle())
		{
			session.c.LogWarning($"Player {session.player.Uid} sent PVEBattleResultCsReq while not in a battle");
			PVEBattleResultScRsp errorRsp = new PVEBattleResultScRsp()
			{
				Retcode = (uint)Retcode.RetBattleFail,
			};
			session.SendPacket(errorRsp);
			return;
		}
		session.player.battleManager.OnBattleResult(req, out PVEBattleResultScRsp rsp);
		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}