using KazusaHSR.Protocol;
using System.Text;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetAuthkeyCsReq
{
	[Packet.PacketCmdId(PacketId.GetAuthkeyCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		var req = packet.GetDecodedBody<GetAuthkeyCsReq>();
		var rsp = new GetAuthkeyScRsp()
		{
			Retcode = 0,
			AuthAppid = req.AuthAppid,
			Authkey = Convert.ToBase64String(Encoding.UTF8.GetBytes("KazusaHsrBestPS")),
			AuthkeyVer = req.AuthkeyVer,
			SignType = req.SignType,
		};

		session.SendPacket(rsp);
	}

}