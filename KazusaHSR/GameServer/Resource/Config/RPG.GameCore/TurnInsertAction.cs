namespace KazusaHSR.GameServer.Resource;

public class TurnInsertAction : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string PrepareAbilityName { get; set; }
}
