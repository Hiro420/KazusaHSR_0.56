namespace KazusaHSR.GameServer.Resource;

public class SetDynamicValueByCharacterCount : TaskConfig
{
	public string DynamicKey { get; set; }
	public TargetEvaluator ReadTargetType { get; set; }
	public PredicateConfig Predicate { get; set; }
}
