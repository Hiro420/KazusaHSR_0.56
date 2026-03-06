namespace KazusaHSR.GameServer.Resource;

public class MoveUINodeToMask : TaskConfig
{
	public string Path { get; set; }
	public string PathPC { get; set; }
	public bool Reset { get; set; }
	public bool Follow { get; set; }
}
