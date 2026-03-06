namespace KazusaHSR.GameServer.Resource;

public class CreatePlayerTeam : TaskConfig
{
	public TeamType TeamType { get; set; }
	public bool Hide { get; set; }
}
