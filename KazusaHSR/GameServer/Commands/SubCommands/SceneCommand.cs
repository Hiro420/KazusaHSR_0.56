using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("scene", "Swtich scene", "sc", RequiresTarget = true)]
public sealed class SceneCommand : IConsoleCommand
{
	public void Execute(CommandExecutionContext context)
	{
		if (context.TargetSession == null || context.TargetUid == null)
		{
			context.Logger.Alert("Give requires a valid target. Use 'target <uid>' first.");
			return;
		}

		if (context.Args.Count == 0)
		{
			context.Logger.Alert("Usage: scene <planeId> <floorId(optional)>");
			return;
		}

		Player player = context.TargetSession.player;
		IEnumerable<MazePlaneRow> mazePlanes = MainApp.resourceManager.MazePlaneExcel;
		uint planeId;
		uint floorId = 0;

		if (!uint.TryParse(context.Args[0], out planeId) || !mazePlanes.Any(f => f.PlaneID == planeId))
		{
			context.Logger.Alert("Invalid plane IDs. Please enter a valid number.");
			return;
		}

		if (context.Args.Count > 1 && !uint.TryParse(context.Args[1], out floorId))
		{
			context.Logger.Alert("Invalid floor ID. Please enter a valid number.");
			return;
		}

		if (context.Args.Count == 1)
		{
			floorId = mazePlanes.First(f => f.PlaneID == planeId).StartFloorID;
		}

		MapEntryRow? mapEntry = MainApp.resourceManager.MapEntranceExcel.FirstOrDefault(m => m.PlaneID == planeId && m.FloorID == floorId);
		if (mapEntry == null)
		{
			context.Logger.Alert("No map entry found for the specified plane and floor ID.");
			return;
		}

		player.EnterMaze(mapEntry, player.GetDefaultGroupForEntry(mapEntry), out Maze? maze);

		EnterMazeByServerScNotify notify = new EnterMazeByServerScNotify
		{
			Maze = maze,
		};
		player.Session.SendPacket(notify);

		player.SavePersistent();
	}
}