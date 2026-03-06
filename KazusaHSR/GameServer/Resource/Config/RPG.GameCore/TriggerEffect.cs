namespace KazusaHSR.GameServer.Resource;

public class TriggerEffect : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string EffectPath { get; set; }
	public string UniqueEffectName { get; set; }
	public string AttachPoint { get; set; }
	public MVector3 PositionOffset { get; set; }
	public MVector3 RotationOffset { get; set; }
	public bool IsAttachToCaster { get; set; }
}
