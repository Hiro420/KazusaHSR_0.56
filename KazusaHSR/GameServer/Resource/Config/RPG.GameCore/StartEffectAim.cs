namespace KazusaHSR.GameServer.Resource;

public class StartEffectAim : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string UniqueEffectName { get; set; }
	public float Duration { get; set; }
}
