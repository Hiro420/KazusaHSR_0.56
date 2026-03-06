namespace KazusaHSR.GameServer.Resource;

public class ByAngle : PredicateConfig
{
	public TargetEvaluator From { get; set; }
	public TargetEvaluator To { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public DynamicFloat CompareValue { get; set; }
}
