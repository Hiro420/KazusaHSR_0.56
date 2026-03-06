namespace KazusaHSR.GameServer.Resource;

public class RemoveAdventureModifier : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string ModifierName { get; set; }
}
