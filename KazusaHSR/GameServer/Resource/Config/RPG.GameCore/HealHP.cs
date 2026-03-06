namespace KazusaHSR.GameServer.Resource;

public class HealHP : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public AFANBEAENPN FormulaType { get; set; }
	public DynamicFloat HealPercentage { get; set; }
	public DynamicFloat SPHitRatio { get; set; }
	public DynamicFloat ModifyValue { get; set; }
	public bool ScreenSpaceFloatMsg { get; set; }
	public DamageDisplayData DisplayData { get; set; }
}
