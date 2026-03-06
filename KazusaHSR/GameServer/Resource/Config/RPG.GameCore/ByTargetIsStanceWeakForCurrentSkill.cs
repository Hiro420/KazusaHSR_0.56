namespace KazusaHSR.GameServer.Resource;

public class ByTargetIsStanceWeakForCurrentSkill : PredicateConfig
{
	public TargetEvaluator AttackerType { get; set; }
	public TargetEvaluator TargetType { get; set; }
}
