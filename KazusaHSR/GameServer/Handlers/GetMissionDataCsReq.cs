using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetMissionDataCsReq
{
	[Packet.PacketCmdId(PacketId.GetMissionDataCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetMissionDataCsReq req = packet.GetDecodedBody<GetMissionDataCsReq>();
		GetMissionDataScRsp rsp = new GetMissionDataScRsp();
		HashSet<uint> uints = new HashSet<uint>();
		foreach (MainMissionRow mission in MainApp.resourceManager.MainMissionExcel)
		{
			// todo: actually store mission data per player
			uints.Add(mission.MainMissionId);
		}
		rsp.FinishedMainMissionIdLists = uints.ToArray(); // had to cuz Append didnt work
		session.SendPacket(rsp);
	}
}