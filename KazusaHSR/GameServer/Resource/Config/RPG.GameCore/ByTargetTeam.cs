namespace KazusaHSR.GameServer.Resource;

public class ByTargetTeam : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public TeamType Team { get; set; }
}
