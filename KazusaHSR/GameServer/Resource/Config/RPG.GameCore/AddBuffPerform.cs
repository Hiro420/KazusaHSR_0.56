namespace KazusaHSR.GameServer.Resource;

public class AddBuffPerform : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public TaskConfig[] TaskList { get; set; }
	public DynamicFloat AddPerformTime { get; set; }
}
