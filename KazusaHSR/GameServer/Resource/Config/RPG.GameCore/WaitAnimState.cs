namespace KazusaHSR.GameServer.Resource;

public class WaitAnimState : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string AnimStateName { get; set; }
	public DynamicFloat NormalizedTimeEnd { get; set; }
}
