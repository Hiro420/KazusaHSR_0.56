namespace KazusaHSR.GameServer.Resource;

public class LevelStartSequeceConfig
{
	public int Order { get; set; }
	public bool IsLoop { get; set; }
	public TaskConfig[] TaskList { get; set; }
}
