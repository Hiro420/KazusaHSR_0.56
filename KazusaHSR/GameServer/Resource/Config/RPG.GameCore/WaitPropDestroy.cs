namespace KazusaHSR.GameServer.Resource;

public class WaitPropDestroy : TaskConfig
{
	public string UniqueName { get; set; }
	public uint GroupID { get; set; }
	public uint GroupPropID { get; set; }
	public TaskConfig[] OnSuccess { get; set; }
}
