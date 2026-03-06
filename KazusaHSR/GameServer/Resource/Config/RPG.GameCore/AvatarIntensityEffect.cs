namespace KazusaHSR.GameServer.Resource;

public class AvatarIntensityEffect : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public float TargetIntensity { get; set; }
	public float FadeDuration { get; set; }
}
