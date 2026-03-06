namespace KazusaHSR.GameServer.Resource;

public class RadialBlurEffect : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool Active { get; set; }
	public float Duration { get; set; }
	public float BlurX { get; set; }
	public float BlurY { get; set; }
	public float BlurRadius { get; set; }
	public int Iteration { get; set; }
	public float BlurStart { get; set; }
	public float BlurFeather { get; set; }
}
