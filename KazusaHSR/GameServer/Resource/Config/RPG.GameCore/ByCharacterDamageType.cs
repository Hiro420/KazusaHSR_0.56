namespace KazusaHSR.GameServer.Resource;

public class ByCharacterDamageType : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public AttackDamageType DamageType { get; set; }
}
