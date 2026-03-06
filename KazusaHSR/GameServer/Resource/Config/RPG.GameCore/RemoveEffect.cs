namespace KazusaHSR.GameServer.Resource;

public class RemoveEffect : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string EffectPath { get; set; }
	public string UniqueEffectName { get; set; }
	public bool IsNeedFadeOut { get; set; }
}
