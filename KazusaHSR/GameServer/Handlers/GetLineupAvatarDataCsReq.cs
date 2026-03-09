using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetLineupAvatarDataCsReq
{
	[Packet.PacketCmdId(PacketId.GetLineupAvatarDataCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetLineupAvatarDataCsReq req = packet.GetDecodedBody<GetLineupAvatarDataCsReq>();
		GetLineupAvatarDataScRsp rsp = new GetLineupAvatarDataScRsp();
		PlayerTeam playerTeam = session.player.GetCurrentLineup();
		foreach (PlayerAvatar avatar in playerTeam.Avatars.Where(a => a != null))
		{
			rsp.AvatarDataLists.Add(new LineupAvatarData()
			{
				AvatarType = AvatarType.AvatarFormalType,
				Id = avatar.Guid,
				Hp = avatar.Hp * 1000, // decimal point :/
			});
		}
		session.SendPacket(rsp);
	}
}