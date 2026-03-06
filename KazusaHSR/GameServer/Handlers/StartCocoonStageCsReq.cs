using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleStartCocoonStageCsReq
{
	[Packet.PacketCmdId(PacketId.StartCocoonStageCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		StartCocoonStageCsReq req = packet.GetDecodedBody<StartCocoonStageCsReq>();
		if (!session.player.Scene.EntityManager.TryGet(req.PropEntityId, out BaseEntity? iEntity) || iEntity is not PropEntity propEntity)
		{
			session.c.LogWarning($"Player {session.player.Uid} tried to start cocoon stage with non-existing prop entity {req.PropEntityId}");
			StartCocoonStageScRsp errorRsp = new StartCocoonStageScRsp()
			{
				Retcode = (uint)Retcode.RetMazePropNotExist,
			};
			session.SendPacket(errorRsp);
			return;
		}
		IEnumerable<CocoonRow> cocoonRows = MainApp.resourceManager.CocoonExcel.Where(c => c.Id == req.CocoonId);
		if (!cocoonRows.Any())
		{
			session.c.LogWarning($"Player {session.player.Uid} tried to start cocoon stage with invalid cocoon ID {req.CocoonId}");
			StartCocoonStageScRsp errorRsp = new StartCocoonStageScRsp()
			{
				Retcode = (uint)Retcode.RetStageNotFound,
			};
			session.SendPacket(errorRsp);
			return;
		}
		CocoonRow? matchingRow = cocoonRows.FirstOrDefault(c => c.WorldLevel == session.player.WorldLevel);
		if (matchingRow == null )
		{
			session.c.LogWarning($"Player {session.player.Uid} tried to start cocoon stage with no matching world level ({session.player.WorldLevel}) for cocoon ID {req.CocoonId}");
			StartCocoonStageScRsp errorRsp = new StartCocoonStageScRsp()
			{
				Retcode = (uint)Retcode.RetStageNotFound,
			};
			session.SendPacket(errorRsp);
			return;
		}
		session.player.battleManager.StartCocoonBattle(matchingRow.StageId, req.PropEntityId);
		StartCocoonStageScRsp rsp = new StartCocoonStageScRsp()
		{
			PropEntityId = req.PropEntityId,
			CocoonId = req.CocoonId,
			BattleInfo = session.player.battleManager.GetCurrentBattleInfo(),
			Wave = 1,
		};
		session.SendPacket(rsp);
	}
}