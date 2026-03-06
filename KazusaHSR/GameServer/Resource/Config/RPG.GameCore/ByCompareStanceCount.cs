namespace KazusaHSR.GameServer.Resource;

public class ByCompareStanceCount : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public DynamicFloat CompareValue { get; set; }
}
