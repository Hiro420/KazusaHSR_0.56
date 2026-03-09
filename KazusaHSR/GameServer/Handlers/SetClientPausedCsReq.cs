using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleSetClientPausedCsReq
{
	[Packet.PacketCmdId(PacketId.SetClientPausedCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		SetClientPausedCsReq req = packet.GetDecodedBody<SetClientPausedCsReq>();
		SetClientPausedScRsp rsp = new SetClientPausedScRsp()
		{
			Paused = req.Paused,
			// todo: actual logic to stop server-side timers and events
		};
		session.SendPacket(rsp);
	}
}