namespace KazusaHSR.GameServer.Resource;

public class ByIsContainAdventureModifier : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string ModifierName { get; set; }
}
