namespace KazusaHSR.GameServer.Resource;

public class HeadLookAt : TaskConfig
{
	public TargetEvaluator PerformerType { get; set; }
	public TargetEvaluator TargetType { get; set; }
	public string TargetAttachPoint { get; set; }
	public bool DoRootRotate { get; set; }
}
