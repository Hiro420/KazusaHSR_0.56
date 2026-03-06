namespace KazusaHSR.GameServer.Resource;

public class RetargetPerSequence : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public PredicateConfig Predicate { get; set; }
	public bool IncludeLimbo { get; set; }
	public LevelStartSequeceConfig[] SequenceList { get; set; }
}
