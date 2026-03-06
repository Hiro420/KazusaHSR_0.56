namespace KazusaHSR.GameServer.Resource;

public class MonsterBehaviour : TaskConfig
{
	public uint InvokeRate { get; set; }
	public TaskConfig[] TaskList { get; set; }
}
