namespace KazusaHSR.GameServer.Resource;

public class HideNonLinkEntity : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool IsHide { get; set; }
}
