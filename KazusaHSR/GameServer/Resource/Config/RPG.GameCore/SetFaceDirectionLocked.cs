namespace KazusaHSR.GameServer.Resource;

public class SetFaceDirectionLocked : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public TargetEvaluator LockTarget { get; set; }
	public bool Lock { get; set; }
}
