using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

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
		rsp.LineupLists.Add(session.player.ChallengeManager.GetLineupInfo());
		session.SendPacket(rsp);
	}
}