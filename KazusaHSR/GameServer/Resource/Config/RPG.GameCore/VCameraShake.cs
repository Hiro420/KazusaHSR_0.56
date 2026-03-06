namespace KazusaHSR.GameServer.Resource;

public class VCameraShake
{
	public bool UseFullPeroid { get; set; }
	public MVector3 Amplitude { get; set; }
	public float Freq { get; set; }
	public MVector3 PositionFreqV3 { get; set; }
	public float ShakeTime { get; set; }
	public MVector3 RotationalAmplitude { get; set; }
	public float RotationalFreq { get; set; }
	public MVector3 RotationFreqV3 { get; set; }
	public MVector3 PerlinNoiseAmplitude { get; set; }
	public MVector3 PerlinNoiseFreq { get; set; }
}
