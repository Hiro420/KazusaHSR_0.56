namespace KazusaHSR.GameServer.Resource;

public class AdventureAbilityConfig : AbilityConfig
{
	public Dictionary<string, AdventureModifierConfig> Modifiers { get; set; } = new();
}
