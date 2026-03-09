using KazusaHSR.Protocol;

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
		session.player.Scene.PostEnterScene();
	}
}