namespace KazusaHSR.GameServer.Resource;

public class ShowWaypoint : TaskConfig
{
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
	public string AssetPath { get; set; }
	public MVector3 Offset { get; set; }
}
