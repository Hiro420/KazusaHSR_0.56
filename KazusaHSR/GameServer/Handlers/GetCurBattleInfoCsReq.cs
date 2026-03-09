using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetCurBattleInfoCsReq
{
	[Packet.PacketCmdId(PacketId.GetCurBattleInfoCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetCurBattleInfoCsReq req = packet.GetDecodedBody<GetCurBattleInfoCsReq>();
		GetCurBattleInfoScRsp rsp = new GetCurBattleInfoScRsp()
		{
			Retcode = (uint)Retcode.RetFail
		};
		// todo: implement battle info retrieval
		session.SendPacket(rsp);
	}
}