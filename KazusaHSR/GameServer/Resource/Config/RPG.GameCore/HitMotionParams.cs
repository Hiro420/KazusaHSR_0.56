namespace KazusaHSR.GameServer.Resource;

public class HitMotionParams
{
	public MVector3 Offset { get; set; }
	public float RiseTime { get; set; }
	public float HangTime { get; set; }
	public float FallTime { get; set; }
	public float HangPeakTime { get; set; }
	public float HangBeginHeightRatio { get; set; }
}
