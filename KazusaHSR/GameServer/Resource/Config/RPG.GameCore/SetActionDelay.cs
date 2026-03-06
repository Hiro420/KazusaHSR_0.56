namespace KazusaHSR.GameServer.Resource;

public class SetActionDelay : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat Value { get; set; }
	public DynamicFloat NormalizedValue { get; set; }
}
