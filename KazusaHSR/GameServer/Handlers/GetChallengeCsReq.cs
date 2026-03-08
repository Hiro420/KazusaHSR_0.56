using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetChallengeCsReq
{
	[Packet.PacketCmdId(PacketId.GetChallengeCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetChallengeCsReq req = packet.GetDecodedBody<GetChallengeCsReq>();
		GetChallengeScRsp rsp = new GetChallengeScRsp()
		{
			Retcode = (uint)Retcode.RetSucc,
		};
		foreach (var row in MainApp.resourceManager.ChallengeMazeExcel)
		{
			uint stars = session.player.ChallengeManager.GetStars(row.ID);
			Challenge challenge = new Challenge
			{
				ChallengeId = row.ID,
				Stars = stars,
			};
			rsp.ChallengeLists.Add(challenge);
		}
		session.SendPacket(rsp);
	}
}