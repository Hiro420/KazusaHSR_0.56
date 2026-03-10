using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetLevelRewardTakenListCsReq
{
	[Packet.PacketCmdId(PacketId.GetLevelRewardTakenListCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetLevelRewardTakenListCsReq req = packet.GetDecodedBody<GetLevelRewardTakenListCsReq>();
		GetLevelRewardTakenListScRsp rsp = new GetLevelRewardTakenListScRsp();

		// todo: store in DB. currently we auto-generate those as we dont store that type of progress
		IEnumerable<uint> generatedList = Enumerable.Range(1, (int)session.player.Level).Select(i => (uint)i);
		rsp.TakenLevelLists = generatedList.ToArray();

		session.SendPacket(rsp);
	}
}