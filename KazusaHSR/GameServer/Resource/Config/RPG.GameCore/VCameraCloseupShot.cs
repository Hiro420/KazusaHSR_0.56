namespace KazusaHSR.GameServer.Resource;

public class VCameraCloseupShot
{
	public bool Exit { get; set; }
	public string CameraTimelineAssetName { get; set; }
	public string CloseupShotPath { get; set; }
	public TargetEvaluator TransTypeFollow { get; set; }
	public MVector3 FollowOffset { get; set; }
	public TargetEvaluator TransTypeAim { get; set; }
	public MVector3 AimOffset { get; set; }
	public bool Override { get; set; }
	public float PauseTime { get; set; }
	public float LightTeamCenterOffset { get; set; }
	public string LightTeamCustomizeFormationName { get; set; }
	public float DarkTeamCenterOffset { get; set; }
	public string DarkTeamCustomizeFormationName { get; set; }
	public float StageRootOffset { get; set; }
}
