namespace KazusaHSR.GameServer.Resource;

public class SetDynamicValue : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string DynamicKey { get; set; }
	public DynamicFloat Value { get; set; }
}
