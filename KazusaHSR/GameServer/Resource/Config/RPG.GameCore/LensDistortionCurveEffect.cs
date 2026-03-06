namespace KazusaHSR.GameServer.Resource;

public class LensDistortionCurveEffect : TaskConfig
{
	public float XMultiplier { get; set; }
	public float YMultiplier { get; set; }
	public float Intensity { get; set; }
	public float Scale { get; set; }
	public string XCurvePath { get; set; }
	public string YCurvePath { get; set; }
	public string IntensityCurvePath { get; set; }
	public string ScaleCurvePath { get; set; }
	public float Duration { get; set; }
}
