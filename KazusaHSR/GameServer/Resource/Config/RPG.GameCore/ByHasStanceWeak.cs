namespace KazusaHSR.GameServer.Resource;

public class ByHasStanceWeak : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public AttackDamageType WeakType { get; set; }
}
