namespace KazusaHSR.GameServer.Resource;

public class SetDynamicValueByProperty : TaskConfig
{
	public string DynamicKey { get; set; }
	public TargetEvaluator ReadTargetType { get; set; }
	public AbilityProperty Value { get; set; }
}
