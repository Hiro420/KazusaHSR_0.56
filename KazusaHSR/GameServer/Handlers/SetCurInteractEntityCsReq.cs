using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleSetCurInteractEntityCsReq
{
	[Packet.PacketCmdId(PacketId.SetCurInteractEntityCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		var req = packet.GetDecodedBody<SetCurInteractEntityCsReq>();
		var rsp = new SetCurInteractEntityScRsp();

		// maybe in future would be useful, for now just skip
		//session.player.CurInteractEntityScRsp = req.EntityId;

		session.SendPacket(rsp);
	}

}