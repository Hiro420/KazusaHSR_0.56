namespace KazusaHSR.GameServer.Resource;

public class AnimationMoveRange
{
	public TargetEvaluator TargetType { get; set; }
	public bool IsBehindTarget { get; set; }
	public bool IgnoreRadius { get; set; }
	public DynamicFloat Speed { get; set; }
	public DynamicFloat NormalizedExitTime { get; set; }
	public DynamicFloat OffsetTargetDistance { get; set; }
	public DynamicFloat OffsetForward { get; set; }
	public DynamicFloat OffsetHorizontal { get; set; }
	public DynamicFloat NormalizedTimeStart { get; set; }
	public DynamicFloat NormalizedTimeEnd { get; set; }
	public DynamicFloat MovePercentage { get; set; }
	public DynamicFloat DefaultMoveDistance { get; set; }
}
