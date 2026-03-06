namespace KazusaHSR.GameServer.Resource;

public class AbilityConfig
{
	public string Name { get; set; }
	public TargetConfig TargetInfo { get; set; }
	public List<TaskConfig> OnAdd { get; set; } = new();
	public List<TaskConfig> OnRemove { get; set; } = new();
	public List<TaskConfig> OnStart { get; set; } = new();
	public List<TaskConfig> OnProjectileHit { get; set; } = new();
	// public OKCPGJLFDDK DynamicValues { get; set; }
}
