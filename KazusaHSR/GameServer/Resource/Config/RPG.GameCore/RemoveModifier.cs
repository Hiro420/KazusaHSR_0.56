namespace KazusaHSR.GameServer.Resource;

public class RemoveModifier : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string ModifierName { get; set; }
	public bool OnlyRemoveCasterAdded { get; set; }
}
