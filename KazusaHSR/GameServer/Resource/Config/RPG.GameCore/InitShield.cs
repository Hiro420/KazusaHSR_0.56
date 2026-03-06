namespace KazusaHSR.GameServer.Resource;

public class InitShield : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat ShieldValue { get; set; }
	public bool ShowText { get; set; }
	public DamageDisplayData DisplayData { get; set; }
}
