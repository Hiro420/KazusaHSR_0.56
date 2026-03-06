using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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