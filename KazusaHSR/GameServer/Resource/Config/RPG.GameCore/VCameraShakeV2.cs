namespace KazusaHSR.GameServer.Resource;

public class VCameraShakeV2
{
	public float ShakeRange { get; set; }
	public float ShakeTime { get; set; }
	public float ShakeDistance { get; set; }
	public CEKOHBBJNNF ShakeSpaceType { get; set; }
	public MVector3 ShakeDir { get; set; }
	public bool BaseOnCamera { get; set; }
	public bool LerpBackAfterShake { get; set; }
	public float BaseCycle { get; set; }
	public float CycleDamping { get; set; }
	public float RangeAttenuation { get; set; }
}
