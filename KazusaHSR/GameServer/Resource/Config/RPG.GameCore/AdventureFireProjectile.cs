namespace KazusaHSR.GameServer.Resource;

public class AdventureFireProjectile : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public ProjectileData Projectile { get; set; }
	public TaskConfig[] OnProjectileHit { get; set; }
	public TaskConfig[] OnProjectileLifetimeFinish { get; set; }
	public bool WaitProjectileFinish { get; set; }
}
