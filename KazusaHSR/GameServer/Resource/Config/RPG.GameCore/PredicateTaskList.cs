namespace KazusaHSR.GameServer.Resource;

public class PredicateTaskList : TaskConfig
{
	public PredicateConfig Predicate { get; set; }
	public TaskConfig[] SuccessTaskList { get; set; }
	public TaskConfig[] FailedTaskList { get; set; }
}
