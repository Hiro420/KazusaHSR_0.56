namespace KazusaHSR.GameServer.Resource;

public class WaitUITabOpen : TaskConfig
{
	public string TabPath { get; set; }
	public string TabPathPC { get; set; }
	public uint Index { get; set; }
	public bool IsOpen { get; set; }
}
