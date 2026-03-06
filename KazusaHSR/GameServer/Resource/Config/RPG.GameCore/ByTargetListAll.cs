namespace KazusaHSR.GameServer.Resource;

public class ByTargetListAll : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public PredicateConfig Predicate { get; set; }
}
