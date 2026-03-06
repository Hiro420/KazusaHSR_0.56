namespace KazusaHSR.GameServer.Resource;

public class SetDynamicValueByDamageDataProperty : TaskConfig
{
	public string DynamicKey { get; set; }
	public TargetEvaluator ReadTargetType { get; set; }
	public PMKNLEJHEBC Property { get; set; }
}
