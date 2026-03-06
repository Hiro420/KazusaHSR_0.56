using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetCurLineupDataCsReq
{
	[Packet.PacketCmdId(PacketId.GetCurLineupDataCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetCurLineupDataCsReq req = packet.GetDecodedBody<GetCurLineupDataCsReq>();
		GetCurLineupDataScRsp rsp = new GetCurLineupDataScRsp()
		{
			Lineup = session.player.GetCurrentLineup().ToTeamProto(),
		};
		session.SendPacket(rsp);
	}
}