namespace KazusaHSR.GameServer.Resource;

public class WaitBattleResult : TaskConfig
{
	public TaskConfig[] WinTaskList { get; set; }
	public TaskConfig[] LoseTaskList { get; set; }
}
