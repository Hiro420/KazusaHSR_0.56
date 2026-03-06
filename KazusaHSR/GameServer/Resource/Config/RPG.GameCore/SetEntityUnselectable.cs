namespace KazusaHSR.GameServer.Resource;

public class SetEntityUnselectable : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool Unselectable { get; set; }
}
