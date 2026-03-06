namespace KazusaHSR.GameServer.Resource;

public class ByIsDamageType : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public AttackDamageType[] DamageTypeList { get; set; }
}
