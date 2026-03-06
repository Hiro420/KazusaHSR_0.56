namespace KazusaHSR.GameServer.Resource;

public class SetHP : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat ModifyValue { get; set; }
	public bool ShowText { get; set; }
	public DamageDisplayData DisplayData { get; set; }
}
