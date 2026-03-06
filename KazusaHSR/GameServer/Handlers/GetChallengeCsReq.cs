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
			// todo
			ChallengeLists = { }
		};
		session.SendPacket(rsp);
	}
}