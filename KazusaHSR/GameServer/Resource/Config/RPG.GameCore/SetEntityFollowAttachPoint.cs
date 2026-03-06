namespace KazusaHSR.GameServer.Resource;

public class SetEntityFollowAttachPoint : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public TargetEvaluator AttachTarget { get; set; }
	public string AttachPoint { get; set; }
}
