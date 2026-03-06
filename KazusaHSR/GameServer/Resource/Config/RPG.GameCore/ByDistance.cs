namespace KazusaHSR.GameServer.Resource;

public class ByDistance : PredicateConfig
{
	public TargetEvaluator From { get; set; }
	public TargetEvaluator To { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public DynamicFloat CompareValue { get; set; }
	public bool IgnoreRadius { get; set; }
}
