using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleSyncTimeCsReq
{
	[Packet.PacketCmdId(PacketId.SyncTimeCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		SyncTimeCsReq req = packet.GetDecodedBody<SyncTimeCsReq>();
		SyncTimeScRsp rsp = new SyncTimeScRsp()
		{
			ServerTimeMs = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds(),
			ClientTimeMs = req.ClientTimeMs,
		};
		session.SendPacket(rsp);
	}
}