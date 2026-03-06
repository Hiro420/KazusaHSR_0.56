namespace KazusaHSR.GameServer.Resource;

public class SetDynamicValueByHPRatio : TaskConfig
{
	public string DynamicKey { get; set; }
	public TargetEvaluator ReadTargetType { get; set; }
}
