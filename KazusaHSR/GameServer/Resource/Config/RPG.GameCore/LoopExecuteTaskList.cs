namespace KazusaHSR.GameServer.Resource;

public class LoopExecuteTaskList : TaskConfig
{
	public DynamicFloat MaxLoopCount { get; set; }
	public TaskConfig[] TaskList { get; set; }
}
