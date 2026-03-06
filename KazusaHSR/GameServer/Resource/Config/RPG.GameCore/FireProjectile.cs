namespace KazusaHSR.GameServer.Resource;

public class FireProjectile : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public PredicateConfig Predicate { get; set; }
	public int TargetMaxHitCount { get; set; }
	public int MaxNumber { get; set; }
	public bool ResetAllHitCount { get; set; }
	public ProjectileData Projectile { get; set; }
	public TaskConfig[] OnProjectileHit { get; set; }
	public bool WaitProjectileFinish { get; set; }
	public DamageDisplayData DisplayData { get; set; }
}
