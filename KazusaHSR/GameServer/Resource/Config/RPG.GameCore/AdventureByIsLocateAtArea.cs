namespace KazusaHSR.GameServer.Resource;

public class AdventureByIsLocateAtArea : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string AreaName { get; set; }
}
