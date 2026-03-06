namespace KazusaHSR.GameServer.Resource;

public class TriggerEffectList : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public EffectConfig[] EffectList { get; set; }
}
