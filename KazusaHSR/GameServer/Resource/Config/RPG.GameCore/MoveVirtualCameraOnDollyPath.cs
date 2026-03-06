namespace KazusaHSR.GameServer.Resource;

public class MoveVirtualCameraOnDollyPath : TaskConfig
{
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
	public float StartPoint { get; set; }
	public float EndPoint { get; set; }
	public string CurveName { get; set; }
	public float Duration { get; set; }
}
