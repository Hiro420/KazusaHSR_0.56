namespace KazusaHSR.GameServer.Resource;

public class RadialBlurCurveEffect : TaskConfig
{
	public float BlurX { get; set; }
	public float BlurY { get; set; }
	public float BlurRadius { get; set; }
	public int Iteration { get; set; }
	public float BlurStart { get; set; }
	public float BlurFeather { get; set; }
	public float Duration { get; set; }
	public string CurveName { get; set; }
}
