namespace KazusaHSR.GameServer.Resource;

public class PropStateExecute : TaskConfig
{
	public bool TargetIsOwner { get; set; }
	public uint GroupPropID { get; set; }
	public uint GroupID { get; set; }
	public DynamicString PropKey { get; set; }
	public TaskConfig[] Execute { get; set; }
	public PropState State { get; set; }
}
