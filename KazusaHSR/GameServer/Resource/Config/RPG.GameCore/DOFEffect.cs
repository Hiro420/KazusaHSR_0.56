namespace KazusaHSR.GameServer.Resource;

public class DOFEffect : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool Active { get; set; }
	public bool WithAnimation { get; set; }
	public float Duration { get; set; }
	public float FocusDistance { get; set; }
	public float BlurUnit { get; set; }
	public float Near { get; set; }
	public float Far { get; set; }
}
