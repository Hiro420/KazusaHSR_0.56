namespace KazusaHSR.GameServer.Resource;

public class ByIsCharacterWeaknessActivated : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public AttackDamageType[] DamageTypeList { get; set; }
}
