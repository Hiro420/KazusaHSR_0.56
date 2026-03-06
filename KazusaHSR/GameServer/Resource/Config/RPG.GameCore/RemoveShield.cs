namespace KazusaHSR.GameServer.Resource;

public class RemoveShield : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool ShowText { get; set; }
}
