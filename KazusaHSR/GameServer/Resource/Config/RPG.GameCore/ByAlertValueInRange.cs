namespace KazusaHSR.GameServer.Resource;

public class ByAlertValueInRange : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public float AlertValueMin { get; set; }
	public float AlertValueMax { get; set; }
}
