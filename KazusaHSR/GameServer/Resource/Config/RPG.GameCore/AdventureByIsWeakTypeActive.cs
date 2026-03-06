namespace KazusaHSR.GameServer.Resource;

public class AdventureByIsWeakTypeActive : PredicateConfig
{
	public TargetEvaluator CasterType { get; set; }
	public TargetEvaluator TargetType { get; set; }
}
