namespace KazusaHSR.GameServer.Resource;

public class AdvBoolPredicateConfig : AdvPredicateConfig
{
	public TaskConfig[] OnSuccess { get; set; }
	public TaskConfig[] OnFailed { get; set; }
}
