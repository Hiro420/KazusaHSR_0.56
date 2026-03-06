namespace KazusaHSR.GameServer.Resource;

public class AdventureTriggerAnimStateWithMove : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public NBKCDMFDBJE AnimLogicState { get; set; }
	public string AnimStateName { get; set; }
	public float NormalizedTimeStart { get; set; }
	public float TransitionDuration { get; set; }
	public bool StopWhenHitOthers { get; set; }
	public AdventureAnimationMoveRange[] MovingRangeList { get; set; }
}
