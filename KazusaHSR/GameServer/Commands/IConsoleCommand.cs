namespace KazusaHSR.GameServer.ConsoleCommands;

public interface IConsoleCommand
{
	void Execute(CommandExecutionContext context);
}