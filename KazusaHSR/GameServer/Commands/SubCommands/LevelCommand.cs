using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("level", "Set adventure level", "lv", RequiresTarget = true)]
public sealed class LevelCommand : IConsoleCommand
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
			context.Logger.Alert("Usage: level <level>");
			return;
		}

		if (!uint.TryParse(context.Args[0], out uint level) || level > 30)
		{
			context.Logger.Alert("Invalid level. Please enter a valid number.");
			return;
		}

		context.TargetSession.player.Level = level;
		PlayerSyncScNotify ntf = new PlayerSyncScNotify()
		{
			BasicInfo = context.TargetSession.player.GetBasicInfo()
		};

		if (context.TargetSession.SendPacket(ntf))
			context.Logger.Emit($"Set player {context.TargetUid} adventure level to {level}.");
		else
			context.Logger.Alert($"Failed to set player {context.TargetUid} adventure level.");

		context.TargetSession.player.SavePersistent();
	}
}