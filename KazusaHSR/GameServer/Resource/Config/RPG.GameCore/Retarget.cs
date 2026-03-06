namespace KazusaHSR.GameServer.Resource;

public class Retarget : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public PredicateConfig Predicate { get; set; }
	public bool IncludeLimbo { get; set; }
	public DynamicFloat MaxNumber { get; set; }
	public TaskConfig[] TaskList { get; set; }
	public TaskConfig[] FailedTaskList { get; set; }
}
