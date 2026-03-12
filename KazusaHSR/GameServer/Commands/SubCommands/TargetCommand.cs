namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("target", "Set or clear the active target session by uid", "t")]
public sealed class TargetCommand : IConsoleCommand
{
	public void Execute(CommandExecutionContext context)
	{
		if (context.Args.Count == 0)
		{
			if (context.Dispatcher.TargetUid.HasValue)
				context.Logger.Message($"Current target UID: {context.Dispatcher.TargetUid.Value}");
			else
				context.Logger.Message("No active target. Use 'target <uid>' to set one.");
			return;
		}

		string arg = context.Args[0];
		if (arg.Equals("clear", StringComparison.OrdinalIgnoreCase) || arg.Equals("none", StringComparison.OrdinalIgnoreCase))
		{
			context.Dispatcher.ClearTarget();
			context.Logger.Message("Target cleared.");
			return;
		}

		if (!uint.TryParse(arg, out uint uid))
		{
			context.Logger.Alert("Usage: target <uid> | target clear");
			return;
		}

		if (!context.Dispatcher.TrySetTarget(uid))
		{
			context.Logger.Alert($"Cannot target UID {uid}: session is offline.");
			return;
		}

		context.Logger.Emit($"Target set to UID {uid}.");
	}
}