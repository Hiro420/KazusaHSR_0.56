namespace KazusaHSR.GameServer.Resource;

public class VCameraBlend
{
	public OKANCKIEAGL BlendType { get; set; }
	public bool UseDefaultBlendCurve { get; set; }
	public string CustomCurveName { get; set; }
	public float BlendTime { get; set; }
	public float FOVSmoothDampTime { get; set; }
	public bool FrameBlendSwitch { get; set; }
	public float FrameBlendDelay { get; set; }
	public float FrameBlendWeightStart { get; set; }
	public float FrameBlendDuration { get; set; }
}
