namespace KazusaHSR.GameServer.Resource;

public class WaveMonsterPerform : TaskConfig
{
	public float PerformMonsterWaitTime { get; set; }
	public TaskConfig[] OnFindPerformMonsterFailed { get; set; }
}
