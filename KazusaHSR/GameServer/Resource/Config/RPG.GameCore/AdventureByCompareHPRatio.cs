namespace KazusaHSR.GameServer.Resource;

public class AdventureByCompareHPRatio : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public float CompareValue { get; set; }
}
