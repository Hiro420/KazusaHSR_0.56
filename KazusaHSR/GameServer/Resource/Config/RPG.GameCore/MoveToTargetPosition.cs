namespace KazusaHSR.GameServer.Resource;

public class MoveToTargetPosition : TaskConfig
{
	public TargetEvaluator PerformerType { get; set; }
	public TargetEvaluator TargetType { get; set; }
	public bool IgnoreRadius { get; set; }
	public DynamicFloat OffsetTargetDistance { get; set; }
	public DynamicFloat MovePercentage { get; set; }
}
