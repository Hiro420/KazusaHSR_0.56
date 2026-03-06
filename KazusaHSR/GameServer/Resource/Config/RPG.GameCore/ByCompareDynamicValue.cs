namespace KazusaHSR.GameServer.Resource;

public class ByCompareDynamicValue : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string DynamicKey { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public DynamicFloat CompareValue { get; set; }
}
