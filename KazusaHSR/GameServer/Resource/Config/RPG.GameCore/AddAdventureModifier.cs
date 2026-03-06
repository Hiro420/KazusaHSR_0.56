namespace KazusaHSR.GameServer.Resource;

public class AddAdventureModifier : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string ModifierName { get; set; }
	public DynamicFloat LifeTime { get; set; }
	public DynamicFloat Count { get; set; }
	public Dictionary<string, DynamicFloat> DynamicValues { get; set; }
}
