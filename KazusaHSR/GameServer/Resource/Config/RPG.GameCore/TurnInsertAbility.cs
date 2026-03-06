namespace KazusaHSR.GameServer.Resource;

public class TurnInsertAbility : TaskConfig
{
	public string AbilityName { get; set; }
	public TargetEvaluator TargetType { get; set; }
	public TargetEvaluator AbilityTarget { get; set; }
	public bool TriggerOnLimbo { get; set; }
	public BEAPKPIOEHJ[] AbortBehaviorFlags { get; set; }
}
