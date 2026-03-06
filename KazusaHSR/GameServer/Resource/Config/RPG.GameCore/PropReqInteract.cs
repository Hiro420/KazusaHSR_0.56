namespace KazusaHSR.GameServer.Resource;

public class PropReqInteract : TaskConfig
{
	public bool TargetIsOwner { get; set; }
	public uint GroupPropID { get; set; }
	public uint GroupID { get; set; }
	public uint InteractID { get; set; }
	public DynamicString PropKey { get; set; }
}
