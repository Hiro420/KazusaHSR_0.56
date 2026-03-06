namespace KazusaHSR.GameServer.Resource;

public class TriggerAbility : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public TargetEvaluator AbilityInherentTargetType { get; set; }
	public PredicateConfig AbilityInherentTargetPredicate { get; set; }
	public string AbilityName { get; set; }
	public bool IsSkillPerform { get; set; }
}
