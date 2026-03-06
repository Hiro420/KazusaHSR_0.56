namespace KazusaHSR.GameServer.Resource;

public class ByCompareAbilityProperty : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public AbilityProperty Property { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public DynamicFloat CompareValue { get; set; }
}
