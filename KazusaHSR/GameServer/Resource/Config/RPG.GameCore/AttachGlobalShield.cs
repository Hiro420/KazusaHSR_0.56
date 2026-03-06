namespace KazusaHSR.GameServer.Resource;

public class AttachGlobalShield : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat InitHP { get; set; }
	public TaskConfig[] OnTargetShieldAdd { get; set; }
	public TaskConfig[] OnTargetShieldRemove { get; set; }
}
