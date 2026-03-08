using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetAvatarDataCsReq
{
	[Packet.PacketCmdId(PacketId.GetAvatarDataCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetAvatarDataCsReq req = packet.GetDecodedBody<GetAvatarDataCsReq>();
		GetAvatarDataScRsp rsp = new GetAvatarDataScRsp();
		IEnumerable<PlayerAvatar> allAvatars = session.player.avatarDict.Values;
		foreach (PlayerAvatar avatar in req.IsGetAll ? allAvatars : allAvatars.Where(i => req.AvatarIdLists.Contains(i.AvatarId)))
		{
			rsp.AvatarLists.Add(avatar.ToAvatarProto());
		}
		session.SendPacket(rsp);
	}
}