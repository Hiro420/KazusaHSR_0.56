namespace KazusaHSR.GameServer.Resource;

public class ByHasSummonRelation : PredicateConfig
{
	public TargetEvaluator SummonerType { get; set; }
	public TargetEvaluator ServantType { get; set; }
}
