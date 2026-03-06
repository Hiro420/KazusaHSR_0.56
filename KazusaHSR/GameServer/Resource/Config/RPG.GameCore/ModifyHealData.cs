namespace KazusaHSR.GameServer.Resource;

public class ModifyHealData : TaskConfig
{
	public DynamicFloat Healer_Attack { get; set; }
	public DynamicFloat Healer_AttackAddedRatio { get; set; }
	public DynamicFloat Healer_AttackDelta { get; set; }
	public DynamicFloat Healer_HealRatio { get; set; }
	public DynamicFloat Target_HealTakenRatio { get; set; }
	public DynamicFloat Task_HealPercentage { get; set; }
	public DynamicFloat Task_HealValue { get; set; }
}
