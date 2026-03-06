namespace KazusaHSR.GameServer.Resource;

public class StackProperty : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public AbilityProperty Property { get; set; }
	public DynamicFloat PropertyValue { get; set; }
}
