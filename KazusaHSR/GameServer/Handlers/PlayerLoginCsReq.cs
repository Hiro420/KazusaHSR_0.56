using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandlePlayerLoginCsReq
{
	[Packet.PacketCmdId(PacketId.PlayerLoginCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		PlayerLoginCsReq req = packet.GetDecodedBody<PlayerLoginCsReq>();
		PlayerLoginScRsp rsp = new PlayerLoginScRsp()
		{
			LoginRandom = req.LoginRandom,
			Stamina = 160,
			ServerTimestampMs = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds(),
			BasicInfo = session.player.GetBasicInfo(),
		};
		session.SendPacket(rsp);
	}
}