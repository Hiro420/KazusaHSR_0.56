namespace KazusaHSR.GameServer.Resource;

public class ComparePropState : TaskConfig
{
	public DynamicFloat GroupID { get; set; }
	public DynamicFloat GroupPropID { get; set; }
	public PropState State { get; set; }
	public TaskConfig[] OnEqual { get; set; }
	public TaskConfig[] OnNotEqual { get; set; }
}
