namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("mp", "Manage team MP", RequiresTarget = true)]
public sealed class MpCommand : IConsoleCommand
{
	public void Execute(CommandExecutionContext context)
	{
		if (context.TargetSession == null || context.TargetUid == null)
		{
			context.Logger.Alert("MP requires a valid target. Use 'target <uid>' first.");
			return;
		}

		var mpManager = context.TargetSession.player.MPManager;

		if (context.Args.Count == 0)
		{
			var state = mpManager.GetCurrentState();
			context.Logger.Emit($"Player {context.TargetUid} MP: {state.Current}/{state.Max}");
			return;
		}

		string action = context.Args[0];
		switch (action.ToLowerInvariant())
		{
			case "refill":
			case "full":
			case "restore":
				{
					uint current = mpManager.RestoreFull();
					uint max = mpManager.GetMaxAmount();
					context.Logger.Emit($"Restored player {context.TargetUid} MP to {current}/{max}.");
					break;
				}

			case "add":
				{
					if (context.Args.Count < 2 || !uint.TryParse(context.Args[1], out uint amount))
					{
						context.Logger.Alert("Usage: mp add <amount>");
						return;
					}

					uint current = mpManager.AddAmount(amount);
					uint max = mpManager.GetMaxAmount();
					context.Logger.Emit($"Added {amount} MP to player {context.TargetUid}. MP is now {current}/{max}.");
					break;
				}

			case "set":
				{
					if (context.Args.Count < 2 || !uint.TryParse(context.Args[1], out uint amount))
					{
						context.Logger.Alert("Usage: mp set <amount>");
						return;
					}

					uint current = mpManager.SetAmount(amount);
					uint max = mpManager.GetMaxAmount();
					context.Logger.Emit($"Set player {context.TargetUid} MP to {current}/{max}.");
					break;
				}

			case "status":
				{
					var state = mpManager.GetCurrentState();
					context.Logger.Emit($"Player {context.TargetUid} MP: {state.Current}/{state.Max}");
					break;
				}

			default:
				context.Logger.Alert("Usage: mp [status|refill|add <amount>|set <amount>]");
				break;
		}
	}
}