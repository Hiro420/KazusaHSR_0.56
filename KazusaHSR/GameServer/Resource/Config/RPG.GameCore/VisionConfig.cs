namespace KazusaHSR.GameServer.Resource;

public class VisionConfig
{
	public string VisionID { get; set; }
	public float DistanceRange { get; set; }
	public float YawAngleRange { get; set; }
	public float PitchAngleLimitMin { get; set; }
	public float PitchAngleLimitMax { get; set; }
	public uint Priority { get; set; }
	public float DistanceAttenuation { get; set; }
	public float AlertValueAddSpeed { get; set; }
	public string RayStartPointName { get; set; }
	public string RayEndPointName { get; set; }
}
