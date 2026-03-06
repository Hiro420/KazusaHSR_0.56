namespace KazusaHSR.GameServer.Resource;

public class ModifierAttachEffect : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string EffectPath { get; set; }
	public string AttachPoint { get; set; }
	public MVector3 PositionOffset { get; set; }
}
