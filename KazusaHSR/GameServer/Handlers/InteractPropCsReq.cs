using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleInteractPropCsReq
{
	[Packet.PacketCmdId(PacketId.InteractPropCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		InteractPropCsReq req = packet.GetDecodedBody<InteractPropCsReq>();
		
		if (!session.player!.Scene!.EntityManager.TryGet(req.PropEntityId, out BaseEntity propEntity))
		{
			session.c.LogWarning($"Player {session.player!.Uid} tried to interact with non-existing prop entity {req.PropEntityId}");
			InteractPropScRsp errorRsp = new InteractPropScRsp()
			{
				Retcode = (uint)Retcode.RetMazePropNotExist,
			};
			session.SendPacket(errorRsp);
			return;
		}

		if (propEntity is not PropEntity prop)
		{
			session.c.LogWarning($"Player {session.player!.Uid} tried to interact with non-prop entity {req.PropEntityId}");
			InteractPropScRsp errorRsp = new InteractPropScRsp()
			{
				Retcode = (uint)Retcode.RetServerInternalError,
			};
			session.SendPacket(errorRsp);
			return;
		}

		InteractRow interactRow = MainApp.resourceManager.InteractExcel.First(i => i.InteractId == req.InteractId);

		// todo: handle item costs. for now just change the state
		if (prop.State == interactRow.SrcState)
		{
			prop.State = interactRow.TargetState;
		}

		InteractPropScRsp rsp = new InteractPropScRsp()
		{
			PropEntityId = propEntity._EntityId,
			PropState = (uint)prop.State,
		};
		session.SendPacket(rsp);
	}
}