using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetTutorialGuideCsReq
{
	[Packet.PacketCmdId(PacketId.GetTutorialGuideCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetTutorialGuideCsReq req = packet.GetDecodedBody<GetTutorialGuideCsReq>();
		GetTutorialGuideScRsp rsp = new GetTutorialGuideScRsp();
		foreach (TutorialGuideRow tutorialRow in MainApp.resourceManager.TutorialGuideExcel)
		{
			rsp.TutorialGuideLists.Add(new TutorialGuide()
			{
				Id = tutorialRow.Id,
				Status = TutorialStatus.TutorialFinish,
			});
		}
		session.SendPacket(rsp);
	}
}