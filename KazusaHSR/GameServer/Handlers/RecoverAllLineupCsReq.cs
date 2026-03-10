using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleRecoverAllLineupCsReq
{
	[Packet.PacketCmdId(PacketId.RecoverAllLineupCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		RecoverAllLineupCsReq req = packet.GetDecodedBody<RecoverAllLineupCsReq>();
		RecoverAllLineupScRsp rsp = new RecoverAllLineupScRsp();
		foreach (PlayerAvatar playerAvatar in session.player.GetCurrentLineup().Avatars)
		{
			AvatarEntity? avatarEntity = session.player.FindEntityByPlayerAvatar(playerAvatar);
			if (avatarEntity == null)
			{
				session.c.LogWarning($"Failed to find avatar entity for player avatar {playerAvatar.Guid} in RecoverAllLineupCsReq");
				continue;
			}
			avatarEntity.RecoverSP();
			avatarEntity.RecoverHp();
		}
		session.SendPacket(rsp);
	}
}