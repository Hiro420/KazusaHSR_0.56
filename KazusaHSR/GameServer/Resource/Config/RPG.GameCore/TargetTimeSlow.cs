namespace KazusaHSR.GameServer.Resource;

public class TargetTimeSlow : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public float TimeScale { get; set; }
	public float UnscaledDuration { get; set; }
}
