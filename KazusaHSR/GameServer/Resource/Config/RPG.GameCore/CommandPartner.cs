namespace KazusaHSR.GameServer.Resource;

public class CommandPartner : TaskConfig
{
	public string MemberName { get; set; }
	public TaskConfig[] TaskList { get; set; }
}
