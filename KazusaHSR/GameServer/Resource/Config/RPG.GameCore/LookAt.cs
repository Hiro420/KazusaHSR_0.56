namespace KazusaHSR.GameServer.Resource;

public class LookAt : TaskConfig
{
	public TargetEvaluator PerformerType { get; set; }
	public NBNOMIIIEHB TargetType { get; set; }
	public float AngleOffset { get; set; }
	public float Duration { get; set; }
}
