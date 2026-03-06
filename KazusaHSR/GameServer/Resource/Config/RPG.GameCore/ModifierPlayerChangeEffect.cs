namespace KazusaHSR.GameServer.Resource;

public class ModifierPlayerChangeEffect : TaskConfig
{
	public float MaxDuration { get; set; }
	public string[] TargetRendererNames { get; set; }
	public FloatCurveFrame[] FloatKeyframes { get; set; }
	public Vector3CurveFrame[] Vector3Keyframes { get; set; }
	public Vector4CurveFrame[] Vector4Keyframes { get; set; }
}
