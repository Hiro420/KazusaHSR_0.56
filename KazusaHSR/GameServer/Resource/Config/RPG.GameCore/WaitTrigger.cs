namespace KazusaHSR.GameServer.Resource;

public class WaitTrigger : TaskConfig
{
	public string TriggerKey { get; set; }
	public TaskConfig[] OnSuccess { get; set; }
}
