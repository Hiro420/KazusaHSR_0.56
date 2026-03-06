using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetTutorialCsReq
{
	[Packet.PacketCmdId(PacketId.GetTutorialCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetTutorialCsReq req = packet.GetDecodedBody<GetTutorialCsReq>();
		GetTutorialScRsp rsp = new GetTutorialScRsp();
		foreach (TutorialRow tutorialRow in MainApp.resourceManager.TutorialExcel)
		{
			rsp.TutorialLists.Add(new Tutorial()
			{
				Id = tutorialRow.TutorialId,
				Status = TutorialStatus.TutorialFinish
			});
		}
		session.SendPacket(rsp);
	}
}