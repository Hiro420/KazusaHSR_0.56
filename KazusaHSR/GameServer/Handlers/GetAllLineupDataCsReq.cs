using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetAllLineupDataCsReq
{
	[Packet.PacketCmdId(PacketId.GetAllLineupDataCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetAllLineupDataCsReq req = packet.GetDecodedBody<GetAllLineupDataCsReq>();
		GetAllLineupDataScRsp rsp = new GetAllLineupDataScRsp()
		{
			CurIndex = session.player.TeamIndex,
		};
		foreach (PlayerTeam team in session.player.TeamManager.Teams)
		{
			rsp.LineupLists.Add(team.ToTeamProto());
		}
		session.SendPacket(rsp);
	}
}