namespace KazusaHSR.GameServer.Resource;

public class AdventureAnimationMoveRange
{
	public TargetEvaluator TargetType { get; set; }
	public bool IgnoreRadius { get; set; }
	public DynamicFloat OffsetTargetDistance { get; set; }
	public DynamicFloat NormalizedTimeStart { get; set; }
	public DynamicFloat NormalizedTimeEnd { get; set; }
	public DynamicFloat MovePercentage { get; set; }
	public DynamicFloat DefaultMoveDistance { get; set; }
	public DynamicFloat MaxMoveDistance { get; set; }
	public bool OnlyMoveForward { get; set; }
	public DynamicFloat MaxSpeed { get; set; }
	public DynamicFloat SteerNormalizedTimeStart { get; set; }
	public DynamicFloat SteerNormalizedTimeEnd { get; set; }
	public DynamicFloat MaxSteerSpeed { get; set; }
}
