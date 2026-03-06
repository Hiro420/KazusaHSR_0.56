using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetCurSceneInfoCsReq
{
	[Packet.PacketCmdId(PacketId.GetCurSceneInfoCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetCurSceneInfoCsReq req = packet.GetDecodedBody<GetCurSceneInfoCsReq>();
		GetCurSceneInfoScRsp rsp = new GetCurSceneInfoScRsp()
		{
			Scene = session.player.Scene.ToSceneInfoProto(),
		};
		session.SendPacket(rsp);
	}
}