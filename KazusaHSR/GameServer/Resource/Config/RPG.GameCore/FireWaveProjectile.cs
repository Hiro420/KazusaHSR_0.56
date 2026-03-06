namespace KazusaHSR.GameServer.Resource;

public class FireWaveProjectile : TaskConfig
{
	public TargetEvaluator CasterTargetType { get; set; }
	public TargetEvaluator TargetType { get; set; }
	public int Count { get; set; }
	public float Interval { get; set; }
	public ProjectileData Projectile { get; set; }
	public bool WaitProjectileFinish { get; set; }
	public DamageByAttackProperty PerProjectileDamage { get; set; }
	public TaskConfig[] OnProjectileHitClientOnly { get; set; }
}
