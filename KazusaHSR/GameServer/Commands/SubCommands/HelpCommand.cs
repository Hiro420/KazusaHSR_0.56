namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("help", "List available console commands", "h", "?")]
public sealed class HelpCommand(Logger logger) : IConsoleCommand
{
	private readonly Logger _logger = logger;

	public void Execute(CommandExecutionContext context)
	{
		if (context.Dispatcher.TargetUid.HasValue)
			_logger.Message($"Active target UID: {context.Dispatcher.TargetUid.Value}");
		else
			_logger.Message("Active target UID: (none)");

		foreach (var cmd in context.Dispatcher.Commands.OrderBy(x => x.Attribute.Name, StringComparer.OrdinalIgnoreCase))
		{
			string aliases = cmd.Attribute.Aliases.Count == 0
				? ""
				: $" (aliases: {string.Join(", ", cmd.Attribute.Aliases)})";
			string targetHint = cmd.Attribute.RequiresTarget ? " [target-required]" : "";
			_logger.Message($"{cmd.Attribute.Name} - {cmd.Attribute.Description}{aliases}{targetHint}");
		}
	}
}