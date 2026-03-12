namespace KazusaHSR.GameServer.ConsoleCommands;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ConsoleCommandAttribute(string name, string description = "", params string[] aliases) : Attribute
{
	public string Name { get; } = name.Trim().ToLowerInvariant();
	public string Description { get; } = description;
	public bool RequiresTarget { get; set; }
	public IReadOnlyList<string> Aliases { get; } = aliases
		.Where(x => !string.IsNullOrWhiteSpace(x))
		.Select(x => x.Trim().ToLowerInvariant())
		.Distinct(StringComparer.OrdinalIgnoreCase)
		.ToArray();
}