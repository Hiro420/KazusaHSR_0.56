namespace KazusaHSR.GameServer.Resource;

public class ModifyActionDelay : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat AddValue { get; set; }
	public DynamicFloat AddNormalizedValue { get; set; }
}
