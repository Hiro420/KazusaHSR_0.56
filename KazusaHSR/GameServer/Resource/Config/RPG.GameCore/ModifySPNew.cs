namespace KazusaHSR.GameServer.Resource;

public class ModifySPNew : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat AddValue { get; set; }
	public DynamicFloat AddRatio { get; set; }
	public DynamicFloat FixedAddValue { get; set; }
}
