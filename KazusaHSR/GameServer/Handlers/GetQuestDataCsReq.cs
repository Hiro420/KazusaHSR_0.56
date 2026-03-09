using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetQuestDataCsReq
{
	[Packet.PacketCmdId(PacketId.GetQuestDataCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetQuestDataCsReq req = packet.GetDecodedBody<GetQuestDataCsReq>();
		GetQuestDataScRsp rsp = new GetQuestDataScRsp();
		// todo: actual logic
		session.SendPacket(rsp);
	}
}