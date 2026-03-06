namespace KazusaHSR.GameServer.Resource;

public class TurnBasedAbilityConfig : AbilityConfig
{
	public Dictionary<string, TurnBasedModifierConfig> Modifiers { get; set; }
}
