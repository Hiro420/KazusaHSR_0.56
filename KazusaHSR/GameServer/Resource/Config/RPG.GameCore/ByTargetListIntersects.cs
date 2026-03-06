namespace KazusaHSR.GameServer.Resource;

public class ByTargetListIntersects : PredicateConfig
{
	public TargetEvaluator FirstTargetType { get; set; }
	public TargetEvaluator SecondTargetType { get; set; }
}
