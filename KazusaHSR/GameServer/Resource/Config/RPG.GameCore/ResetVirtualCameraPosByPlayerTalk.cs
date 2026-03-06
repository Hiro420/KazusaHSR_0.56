namespace KazusaHSR.GameServer.Resource;

public class ResetVirtualCameraPosByPlayerTalk : TaskConfig
{
	public string AreaName { get; set; }
	public string CameraAnchorName { get; set; }
	public string PlayerAnchorName { get; set; }
	public string NPCAnchorName { get; set; }
	public string RealPlayerUniqueName { get; set; }
	public string RealNPCUniqueName { get; set; }
}
