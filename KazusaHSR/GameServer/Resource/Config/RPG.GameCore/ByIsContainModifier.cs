namespace KazusaHSR.GameServer.Resource;

public class ByIsContainModifier : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string ModifierName { get; set; }
}
