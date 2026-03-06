namespace KazusaHSR.GameServer.Resource;

public class SetAttachmentVisibility : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public AttachmentVisibilityNode[] AttachPoints { get; set; }
	public bool Visibility { get; set; }
	public int VisibilityApplyDelay { get; set; }
}
