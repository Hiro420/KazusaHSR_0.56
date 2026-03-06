namespace KazusaHSR.GameServer.Resource;

public class StartAim : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string AimerName { get; set; }
	public string TargetAttachPoint { get; set; }
	public float TransitTime { get; set; }
	public float VerticalAlpha { get; set; }
	public float HorizontalAlpha { get; set; }
	public bool IsEffect { get; set; }
	public string EffectPath { get; set; }
}
