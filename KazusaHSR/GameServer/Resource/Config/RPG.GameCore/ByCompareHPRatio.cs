namespace KazusaHSR.GameServer.Resource;

public class ByCompareHPRatio : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public DynamicFloat CompareValue { get; set; }
}
