namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("list", "List all active users", "who", "online")]
public sealed class ListCommand : IConsoleCommand
{
	public void Execute(CommandExecutionContext context)
	{
		var sessions = context.Dispatcher.GetActivePlayerSessions();
		if (sessions.Count == 0)
		{
			context.Logger.Message("No active users.");
			return;
		}

		context.Logger.Message($"Active users ({sessions.Count}):");
		foreach (var session in sessions)
		{
			context.Logger.Message($"- UID {session.player.Uid} | Name {session.player.Name}");
		}
	}
}