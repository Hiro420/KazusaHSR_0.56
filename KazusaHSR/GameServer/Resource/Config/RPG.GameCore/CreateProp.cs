namespace KazusaHSR.GameServer.Resource;

public class CreateProp : TaskConfig
{
	public uint PropID { get; set; }
	public uint GroupID { get; set; }
	public uint GroupPropID { get; set; }
	public string UniqueName { get; set; }
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
	public string AnchorNativeProp { get; set; }
	public TeamType Team { get; set; }
	public float Duration { get; set; }
	public JNCHJAFHMMI CampID { get; set; }
	public TaskConfig[] OnCreate { get; set; }
}
