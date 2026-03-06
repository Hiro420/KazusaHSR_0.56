using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleWaitCustomStringCsReq
{
	[Packet.PacketCmdId(PacketId.WaitCustomStringCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{

		// TODO: handle by TaskConfig

		WaitCustomStringCsReq req = packet.GetDecodedBody<WaitCustomStringCsReq>();
		WaitCustomStringScRsp rsp = new WaitCustomStringScRsp()
		{
			CustomString = req.CustomString
		};

		// had to, cuz its a oneof field
		if (req.PropEntityId != 0)
		{
			rsp.PropEntityId = req.PropEntityId;
		}
		else if (req.SubMissionId != 0)
		{
			rsp.SubMissionId = req.SubMissionId;
		}
		session.SendPacket(rsp);
	}
}