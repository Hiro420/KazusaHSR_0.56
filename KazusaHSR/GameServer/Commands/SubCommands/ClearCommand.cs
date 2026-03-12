namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("clear", "Clear the terminal", "cls")]
public sealed class ClearCommand : IConsoleCommand
{
	public void Execute(CommandExecutionContext context)
	{
		Console.Clear();
	}
}