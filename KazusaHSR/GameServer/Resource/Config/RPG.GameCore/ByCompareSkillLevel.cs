namespace KazusaHSR.GameServer.Resource;

public class ByCompareSkillLevel : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string TriggerKey { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public DynamicFloat CompareValue { get; set; }
}
