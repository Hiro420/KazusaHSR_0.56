namespace KazusaHSR.GameServer.Resource;

public class SetCameraRootFollow : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string PointName { get; set; }
	public bool IsReset { get; set; }
}
