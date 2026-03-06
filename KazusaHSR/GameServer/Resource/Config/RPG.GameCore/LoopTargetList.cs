namespace KazusaHSR.GameServer.Resource;

public class LoopTargetList : TaskConfig
{
	public DynamicFloat MaxLoopCount { get; set; }
	public TaskConfig[] TaskList { get; set; }
}
