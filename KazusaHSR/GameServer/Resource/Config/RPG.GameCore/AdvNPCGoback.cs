namespace KazusaHSR.GameServer.Resource;

public class AdvNPCGoback : TaskConfig
{
	public TaskConfig OnBeforeGoBack { get; set; }
	public TaskConfig OnBackToGuardPoint { get; set; }
}
