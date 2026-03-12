namespace KazusaHSR.GameServer.ConsoleCommands;

public sealed class CommandExecutionContext(
	string rawInput,
	string commandName,
	IReadOnlyList<string> args,
	Logger logger,
	ConsoleCommandDispatcher dispatcher,
	uint? targetUid,
	Session? targetSession)
{
	public string RawInput { get; } = rawInput;
	public string CommandName { get; } = commandName;
	public IReadOnlyList<string> Args { get; } = args;
	public Logger Logger { get; } = logger;
	public ConsoleCommandDispatcher Dispatcher { get; } = dispatcher;
	public uint? TargetUid { get; } = targetUid;
	public Session? TargetSession { get; } = targetSession;
}