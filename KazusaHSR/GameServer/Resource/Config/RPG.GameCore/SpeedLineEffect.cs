namespace KazusaHSR.GameServer.Resource;

public class SpeedLineEffect : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public HCILFEENCKM Mode { get; set; }
	public bool Active { get; set; }
	public float Speed { get; set; }
	public bool UseTwoTone { get; set; }
	public float WhitePercent { get; set; }
	public float Divide { get; set; }
	public float Offset { get; set; }
	public float Start { get; set; }
	public float End { get; set; }
	public float Brightness { get; set; }
	public float Density { get; set; }
	public float Rotation { get; set; }
	public float PosX { get; set; }
	public float PosY { get; set; }
}
