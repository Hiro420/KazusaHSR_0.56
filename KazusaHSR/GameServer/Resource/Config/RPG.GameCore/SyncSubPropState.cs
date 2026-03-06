namespace KazusaHSR.GameServer.Resource;

public class SyncSubPropState : TaskConfig
{
	public DynamicFloat MainGroupID { get; set; }
	public DynamicFloat MainGroupPropID { get; set; }
	public PropState MainState { get; set; }
	public DynamicFloat SubGroupID { get; set; }
	public DynamicFloat SubGroupPropID { get; set; }
	public PropState SubState { get; set; }
}
