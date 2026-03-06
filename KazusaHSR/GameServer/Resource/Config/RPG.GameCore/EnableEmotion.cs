namespace KazusaHSR.GameServer.Resource;

public class EnableEmotion : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool Enable { get; set; }
}
