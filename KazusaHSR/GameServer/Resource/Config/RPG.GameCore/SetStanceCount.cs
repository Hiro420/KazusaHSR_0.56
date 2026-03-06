namespace KazusaHSR.GameServer.Resource;

public class SetStanceCount : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool IsReset { get; set; }
	public DynamicFloat AddCustomValue { get; set; }
}
