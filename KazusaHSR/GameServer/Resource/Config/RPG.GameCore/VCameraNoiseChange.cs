namespace KazusaHSR.GameServer.Resource;

public class VCameraNoiseChange : TaskConfig
{
	public bool Reset { get; set; }
	public float AmplitudeGain { get; set; }
	public float FrequencyGain { get; set; }
	public string ProfilePath { get; set; }
}
