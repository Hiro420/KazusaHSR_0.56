namespace KazusaHSR.GameServer.Resource;

public class WaitTurnCount : TaskConfig
{
	public GEEJGCFFONO CalcMethod { get; set; }
	public uint Count { get; set; }
	public TaskConfig[] TaskList { get; set; }
}
