using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetQuestDataCsReq
{
	[Packet.PacketCmdId(PacketId.GetQuestDataCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetQuestDataCsReq req = packet.GetDecodedBody<GetQuestDataCsReq>();
		GetQuestDataScRsp rsp = new GetQuestDataScRsp();

		// todo: actually store in db and send real data
		foreach (QuestDataRow row in MainApp.resourceManager.QuestData)
		{
			FinishWayRow finishWay = MainApp.resourceManager.FinishWay.First(i => i.ID == row.FinishWayID);
			Quest quest = new Quest
			{
				Id = row.QuestID,
				Status = QuestStatus.QuestClose,
				Progress = finishWay.Progress != 0 ? finishWay.Progress : row.UnlockProgress
			};
			rsp.QuestLists.Add(quest);
		}

		session.SendPacket(rsp);
	}
}