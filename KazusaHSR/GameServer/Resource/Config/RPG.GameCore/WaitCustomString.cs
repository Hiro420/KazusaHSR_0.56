namespace KazusaHSR.GameServer.Resource;

public class WaitCustomString : TaskConfig
{
	public string CustomString { get; set; }
	public bool WaitOwnerOnly { get; set; }
	public bool Sync { get; set; }
}
