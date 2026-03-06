using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.Handlers.Recv;

internal class HandleGetMazeMapInfoCsReq
{
	[Packet.PacketCmdId(PacketId.GetMazeMapInfoCsReq)]
	public static void OnPacket(Session session, Packet packet)
	{
		GetMazeMapInfoCsReq req = packet.GetDecodedBody<GetMazeMapInfoCsReq>();
		MapEntranceRow? row = MainApp.resourceManager.MapEntranceExcel.FirstOrDefault(m => m.Id == req.EntryId);
		if (row == null)
		{
			session.c.LogWarning($"Player {session.player!.Uid} requested maze map info for invalid EntryId {req.EntryId}");
			GetMazeMapInfoScRsp errorRsp = new GetMazeMapInfoScRsp()
			{
				Retcode = (uint)Retcode.RetSceneEntryIdNotMatch,
			};
			session.SendPacket(errorRsp);
			return;
		}

		uint floorId = row.FloorId;
		uint planeId = row.PlaneId;

		Dictionary<uint, LevelGroupInfo> levelGroups = Scene.GetDefaultLevelGroups(planeId, floorId);

		GetMazeMapInfoScRsp rsp = new GetMazeMapInfoScRsp()
		{
			LightenSectionLists = Enumerable.Range(0, 32).Select(i => (uint)i).ToArray(),
			EntryId = req.EntryId
		};
		foreach (KeyValuePair<uint, LevelGroupInfo> kvp in levelGroups)
		{
			rsp.MazeGroupLists.Add(new MazeGroup()
			{
				GroupId = kvp.Key,
				ModifyTime = 0, // TODO: implement modify time tracking
			});
			foreach (LevelPropInfo propInfo in kvp.Value.PropList ?? Enumerable.Empty<LevelPropInfo>())
			{
				rsp.MazePropLists.Add(new MazePropState()
				{
					ConfigId = propInfo.ID,
					GroupId = kvp.Key,
					State = (uint)propInfo.State, // todo: track state changes
				});
			}
		}
		session.SendPacket(rsp);
	}
}