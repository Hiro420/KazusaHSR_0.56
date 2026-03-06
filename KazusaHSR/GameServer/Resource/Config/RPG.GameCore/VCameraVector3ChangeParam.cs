namespace KazusaHSR.GameServer.Resource;

public class VCameraVector3ChangeParam
{
	public MVector3 TargetValue { get; set; }
	public float Time { get; set; }
	public string ChangeCurvePath { get; set; }
	public float RecoveryTime { get; set; }
	public string RecoveryCurvePath { get; set; }
}
