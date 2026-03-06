namespace KazusaHSR.GameServer.Resource;

public class VCameraConfig
{
	public IJECFOJCLPJ CameraState { get; set; }
	public VCameraNormalConfig NormalConfig { get; set; }
	public VCameraNormalConfig AdditiveNormalConfig { get; set; }
	public VCameraFreelook3rdConfig Freelook3rdConfig { get; set; }
	public VCameraShotAnimMovementConfig ShotAnimMovementConfig { get; set; }
	public string ShakeTemplateName { get; set; }
	public VCameraShake ShakeConfig { get; set; }
	public VCameraShakeV2 OverrideShakeConfigV2 { get; set; }
	public VCameraCloseupShot CloseupShotConfig { get; set; }
	public VCameraBlend BlendConfig { get; set; }
	public VCameraShowTargetEntity ShowEntityConfig { get; set; }
	public VCameraStateAdditiveNormalConfig StateAdditiveNormalConfig { get; set; }
}
