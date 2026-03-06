namespace KazusaHSR.GameServer.Resource;

public class AdvSetTargetAlertValue : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DEIGACAIGJA BehaviourType { get; set; }
	public float AlertValue { get; set; }
}
