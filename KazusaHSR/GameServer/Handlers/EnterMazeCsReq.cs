using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleEnterMazeCsReq
{
	[Packet.PacketCmdId(PacketId.EnterMazeCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		EnterMazeCsReq req = packet.GetDecodedBody<EnterMazeCsReq>();
		EnterMazeScRsp rsp = new EnterMazeScRsp();
		
		MapEntranceRow? entrance = MainApp.resourceManager.MapEntranceExcel.FirstOrDefault(row => row.Id == req.EntryId);

		// little sanity check
		if (entrance == null)
		{
			session.c.LogError($"Player {session.player!.Uid} tried to enter maze with invalid entrance ID {req.EntryId}.");
			rsp.Retcode = (uint)Retcode.RetSceneEntryIdNotMatch;
			session.SendPacket(rsp);
		}

		session.player!.EnterMaze(entrance!, req.GroupId, req.ConfigId, out Maze? maze);
		
		if (maze == null)
		{
			rsp.Retcode = (uint)Retcode.RetServerInternalError;
			session.SendPacket(rsp);
			return;
		}

		rsp.Retcode = 0;
		rsp.Maze = maze;

		session.SendPacket(rsp);
		session.player.SavePersistent();
	}
}