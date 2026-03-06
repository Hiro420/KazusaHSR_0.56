namespace KazusaHSR.GameServer.Resource;

public class VCameraNormalConfig
{
	public IJECFOJCLPJ CameraState { get; set; }
	public string TemplateName { get; set; }
	public FJCEKCNPOFN AttackType { get; set; }
	public TargetEvaluator AnchorTargetType { get; set; }
	public bool IsLocalOffset { get; set; }
	public MVector3 AnchorOffset { get; set; }
	public float AnchorRatio { get; set; }
	public TargetEvaluator AimTargetType { get; set; }
	public MVector3 AimOffset { get; set; }
	public float AimRatio { get; set; }
	public float FollowPoleAngle { get; set; }
	public float FollowElevationAngle { get; set; }
	public float FollowRadius { get; set; }
	public float AnchorToAimAngle { get; set; }
	public float FollowDamp { get; set; }
	public float AimDamp { get; set; }
	public float Dutch { get; set; }
	public float FOV { get; set; }
	public float MoveForwardDis { get; set; }
	public bool ForbidDynamicOffset { get; set; }
	public bool ResetToDefault { get; set; }
}
