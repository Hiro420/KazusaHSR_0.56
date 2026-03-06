namespace KazusaHSR.GameServer.Resource;

public class MoveToTargetList : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public LNCCACEHFIO AnimLogicState { get; set; }
	public string AnimStateName { get; set; }
	public DynamicFloat NormalizedTimeStart { get; set; }
	public DynamicFloat NormalizedTimeEnd { get; set; }
	public DynamicFloat NormalizedTransitionDuration { get; set; }
	public DynamicFloat NormalizedTimeWait { get; set; }
	public AnimationMoveRange[] MovingRangeList { get; set; }
}
