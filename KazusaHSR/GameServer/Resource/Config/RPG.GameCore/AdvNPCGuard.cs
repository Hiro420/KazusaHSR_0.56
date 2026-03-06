namespace KazusaHSR.GameServer.Resource;

public class AdvNPCGuard : TaskConfig
{
	public float Duration { get; set; }
	public TaskConfig GuardEnterTask { get; set; }
	public TaskConfig GuardExitTask { get; set; }
}
