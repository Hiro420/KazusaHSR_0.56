namespace KazusaHSR.GameServer.Resource;

public class AdventureTriggerAttack : TaskConfig
{
	public TargetEvaluator AttackTargetType { get; set; }
	public TargetEvaluator AttackRootTargetType { get; set; }
	public bool TriggerBattle { get; set; }
	public float TriggerBattleDelay { get; set; }
	public AdventureAttackDetectShapeConfig AttackDetectConfig { get; set; }
	public AdventureHitConfig HitConfig { get; set; }
	public TaskConfig[] OnAttack { get; set; }
}
