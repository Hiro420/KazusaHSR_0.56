namespace KazusaHSR.GameServer.Resource;

public class VCameraConfigChange : TaskConfig
{
	public bool WaitCloseupShotFinish { get; set; }
	public string CharacterCameraConfigName { get; set; }
	public TargetEvaluator CharacterCameraTargetType { get; set; }
	public VCameraConfig CameraConfig { get; set; }
}
