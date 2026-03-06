namespace KazusaHSR.GameServer.Resource;

public class ReviveTeamer : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool MaxState { get; set; }
}
