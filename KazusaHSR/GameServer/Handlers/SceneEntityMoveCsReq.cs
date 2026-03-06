using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleSceneEntityMoveCsReq
{
	[Packet.PacketCmdId(PacketId.SceneEntityMoveCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		SceneEntityMoveCsReq req = packet.GetDecodedBody<SceneEntityMoveCsReq>();
		foreach (var motion in req.EntityMotionLists)
		{
			if (session.player.Scene.EntityManager.TryGet(motion.EntityId, out BaseEntity? baseEntity))
			{
				baseEntity.Position = motion.Motion.Pos;
				baseEntity.Rotation = motion.Motion.Rot;

				if (baseEntity is AvatarEntity)
				{
					session.player.SetPos(motion.Motion.Pos);
					session.player.SetRot(motion.Motion.Rot);
					session.player.Scene.EntranceId = req.EntryId;
					session.player.SavePersistent();
				}
			}

		}
		session.SendPacket(new SceneEntityMoveScRsp());
	}
}