namespace KazusaHSR.GameServer.Resource;

public class AddMazeBuff : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public uint ID { get; set; }
	public PredicateConfig Condition { get; set; }
	public TargetEvaluator BindingAffectedTarget { get; set; }
	public DynamicFloat LifeTime { get; set; }
	public DynamicFloat Count { get; set; }
	public Dictionary<string, DynamicFloat> DynamicValues { get; set; }
}
