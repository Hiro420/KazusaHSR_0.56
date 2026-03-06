namespace KazusaHSR.GameServer.Resource;

public class ByCompareStance : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public DynamicFloat CompareValue { get; set; }
}
