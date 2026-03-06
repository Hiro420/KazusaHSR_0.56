namespace KazusaHSR.GameServer.Resource;

public class ChangePropState : TaskConfig
{
	public DynamicFloat DynamicGroupPropID { get; set; }
	public DynamicFloat DynamicGroupID { get; set; }
	public PropState State { get; set; }
}
