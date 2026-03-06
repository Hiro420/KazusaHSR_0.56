namespace KazusaHSR.GameServer.Resource;

public class LoseHPByRatio : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public CJCEPOOKBIM RatioType { get; set; }
	public DynamicFloat Ratio { get; set; }
	public string DynamicFloatSet { get; set; }
	public DamageDisplayData DisplayData { get; set; }
}
