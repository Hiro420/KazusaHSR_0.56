namespace KazusaHSR.GameServer.Resource;

public class ByTargetListAny : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public PredicateConfig Predicate { get; set; }
}
