namespace KazusaHSR.GameServer.Resource;

public class AdvNPCGuarding : TaskConfig
{
	public bool EnableSearch { get; set; }
	public float SearchOffset { get; set; }
	public TaskConfig OnGuardStart { get; set; }
	public TaskConfig OnGuardEnd { get; set; }
}
