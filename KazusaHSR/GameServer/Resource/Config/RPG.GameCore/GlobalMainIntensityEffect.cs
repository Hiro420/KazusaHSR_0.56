namespace KazusaHSR.GameServer.Resource;

public class GlobalMainIntensityEffect : TaskConfig
{
	public float TargetIntensity { get; set; }
	public float FadeDuration { get; set; }
	public float TartgetFogDensity { get; set; }
	public int[] FogColor { get; set; }
	public float FogNear { get; set; }
	public float FogFar { get; set; }
}
