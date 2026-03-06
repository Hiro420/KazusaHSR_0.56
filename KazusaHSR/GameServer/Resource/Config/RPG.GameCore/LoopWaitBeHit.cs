namespace KazusaHSR.GameServer.Resource;

public class LoopWaitBeHit : TaskConfig
{
	public DynamicFloat GroupPropID { get; set; }
	public TaskConfig[] OnBeHit { get; set; }
}
