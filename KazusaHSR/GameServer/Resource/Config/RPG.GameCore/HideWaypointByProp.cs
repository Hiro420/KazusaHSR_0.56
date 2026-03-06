namespace KazusaHSR.GameServer.Resource;

public class HideWaypointByProp : TaskConfig
{
	public bool UseOwnerEntity { get; set; }
	public uint GroupID { get; set; }
	public uint InstanceID { get; set; }
}
