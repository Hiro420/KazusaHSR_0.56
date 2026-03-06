namespace KazusaHSR.GameServer.Resource;

public class DamageByAttackProperty : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public AttackData AttackProperty { get; set; }
	public bool TriggerHitSound { get; set; }
	public string SpecialHitSoundEvent { get; set; }
	public bool CanTriggerLastKill { get; set; }
	public string EffectMessage { get; set; }
	public AttackType AttackType { get; set; }
	public DamageDisplayData DisplayData { get; set; }
}
