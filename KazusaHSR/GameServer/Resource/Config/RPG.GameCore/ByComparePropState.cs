namespace KazusaHSR.GameServer.Resource;

public class ByComparePropState : PredicateConfig
{
	public bool TargetIsOwner { get; set; }
	public uint GroupPropID { get; set; }
	public uint GroupID { get; set; }
	public PropState State { get; set; }
}
