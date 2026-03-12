using System.Reflection;
using System.Text;

namespace KazusaHSR.GameServer.ConsoleCommands;

public sealed class ConsoleCommandDispatcher : IDisposable
{
	private readonly Logger _logger;
	private readonly Dictionary<string, CommandRegistration> _commandLookup = new(StringComparer.OrdinalIgnoreCase);
	private readonly List<CommandRegistration> _registrations = [];
	private uint? _targetUid;

	public ConsoleCommandDispatcher(Logger logger)
	{
		_logger = logger;
		LoadFromAssembly(Assembly.GetExecutingAssembly());
		IConsole.OnCommandExecuted += HandleInput;
		_logger.Emit($"Loaded {_registrations.Count} console commands.");
	}

	public IReadOnlyList<CommandRegistration> Commands => _registrations;
	public uint? TargetUid => _targetUid;

	public void Dispose()
	{
		IConsole.OnCommandExecuted -= HandleInput;
	}

	private void LoadFromAssembly(Assembly assembly)
	{
		var candidates = assembly
			.GetTypes()
			.Where(t => !t.IsAbstract && !t.IsInterface && typeof(IConsoleCommand).IsAssignableFrom(t));

		foreach (var type in candidates)
		{
			var attr = type.GetCustomAttribute<ConsoleCommandAttribute>();
			if (attr == null)
				continue;

			var instance = CreateCommandInstance(type);
			var registration = new CommandRegistration(type, attr, instance);
			_registrations.Add(registration);
			BindName(attr.Name, registration);
			foreach (var alias in attr.Aliases)
				BindName(alias, registration);
		}
	}

	private IConsoleCommand CreateCommandInstance(Type type)
	{
		var loggerCtor = type.GetConstructor([typeof(Logger)]);
		if (loggerCtor != null)
			return (IConsoleCommand)loggerCtor.Invoke([_logger]);

		var emptyCtor = type.GetConstructor(Type.EmptyTypes);
		if (emptyCtor != null)
			return (IConsoleCommand)emptyCtor.Invoke([]);

		throw new InvalidOperationException($"Command type {type.FullName} must have a parameterless constructor or Logger constructor.");
	}

	private void BindName(string name, CommandRegistration registration)
	{
		if (_commandLookup.TryGetValue(name, out var existing))
		{
			_logger.Alert($"Skipped duplicate command binding '{name}' from {registration.Type.Name}; already bound by {existing.Type.Name}.");
			return;
		}

		_commandLookup[name] = registration;
	}

	private void HandleInput(string input)
	{
		var parts = SplitArgs(input);
		if (parts.Count == 0)
			return;

		var commandName = parts[0].ToLowerInvariant();
		if (!_commandLookup.TryGetValue(commandName, out var registration))
		{
			_logger.Alert($"Unknown command '{commandName}'. Try 'help'.");
			return;
		}

		Session? targetSession = null;
		if (_targetUid.HasValue)
			targetSession = TryGetActiveSessionByUid(_targetUid.Value);

		if (registration.Attribute.RequiresTarget)
		{
			if (!_targetUid.HasValue)
			{
				_logger.Alert($"Command '{commandName}' requires a selected target. Use 'target <uid>' first.");
				return;
			}

			if (targetSession == null)
			{
				_logger.Alert($"Target UID {_targetUid.Value} is no longer online. Clear or retarget before running '{commandName}'.");
				return;
			}
		}

		var args = parts.Skip(1).ToArray();
		var context = new CommandExecutionContext(input, commandName, args, _logger, this, _targetUid, targetSession);
		try
		{
			registration.Instance.Execute(context);
		}
		catch (Exception ex)
		{
			_logger.Fail($"Command '{commandName}' failed: {ex.Message}\n{ex.StackTrace}");
		}
	}

	public Session? TryGetActiveSessionByUid(uint uid)
	{
		return GameServerManager.sessions.FirstOrDefault(s => s.player != null && s.player.Uid == uid);
	}

	public bool TrySetTarget(uint uid)
	{
		var session = TryGetActiveSessionByUid(uid);
		if (session == null)
			return false;

		_targetUid = uid;
		return true;
	}

	public void ClearTarget()
	{
		_targetUid = null;
	}

	public IReadOnlyList<Session> GetActivePlayerSessions()
	{
		return GameServerManager.sessions
			.Where(s => s.player != null)
			.OrderBy(s => s.player.Uid)
			.ToList();
	}

	private static List<string> SplitArgs(string input)
	{
		var output = new List<string>();
		var token = new StringBuilder();
		bool inQuotes = false;

		for (int i = 0; i < input.Length; i++)
		{
			char c = input[i];
			if (c == '"')
			{
				inQuotes = !inQuotes;
				continue;
			}

			if (!inQuotes && char.IsWhiteSpace(c))
			{
				if (token.Length > 0)
				{
					output.Add(token.ToString());
					token.Clear();
				}
				continue;
			}

			token.Append(c);
		}

		if (token.Length > 0)
			output.Add(token.ToString());

		return output;
	}

	public sealed class CommandRegistration(Type type, ConsoleCommandAttribute attribute, IConsoleCommand instance)
	{
		public Type Type { get; } = type;
		public ConsoleCommandAttribute Attribute { get; } = attribute;
		public IConsoleCommand Instance { get; } = instance;
	}
}
