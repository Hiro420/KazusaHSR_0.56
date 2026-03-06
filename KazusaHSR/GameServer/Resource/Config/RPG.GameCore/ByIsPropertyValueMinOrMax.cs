namespace KazusaHSR.GameServer.Resource;

public class ByIsPropertyValueMinOrMax : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public TargetEvaluator CompareTargetType { get; set; }
	public HILGLGJKLEN MinOrMax { get; set; }
	public AbilityProperty PropertyType { get; set; }
	public AbilityPropertyRatio PropertyRatioType { get; set; }
}
