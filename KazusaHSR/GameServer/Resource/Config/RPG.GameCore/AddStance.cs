namespace KazusaHSR.GameServer.Resource;

public class AddStance : TaskConfig
{
	public bool RemoveFlag { get; set; }
	public DynamicFloat MaxStance { get; set; }
	public int Count { get; set; }
	public TargetEvaluator TargetType { get; set; }
}
