namespace KazusaHSR.GameServer.Resource;

public class ByIsSkillTypeDisable : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public SkillType Type { get; set; }
}
