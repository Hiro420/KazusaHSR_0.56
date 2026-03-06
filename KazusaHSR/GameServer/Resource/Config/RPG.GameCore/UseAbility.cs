namespace KazusaHSR.GameServer.Resource;

public class UseAbility : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string AbilityName { get; set; }
}
