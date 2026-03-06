namespace KazusaHSR.GameServer.Resource;

public class ShowWaypointByProp : TaskConfig
{
	public bool UseOwnerEntity { get; set; }
	public uint GroupID { get; set; }
	public uint InstanceID { get; set; }
	public uint MinRange { get; set; }
	public uint MaxRange { get; set; }
	public string AssetPath { get; set; }
	public string IconPath { get; set; }
	public MVector3 Offset { get; set; }
	public bool OnNameBoard { get; set; }
}
