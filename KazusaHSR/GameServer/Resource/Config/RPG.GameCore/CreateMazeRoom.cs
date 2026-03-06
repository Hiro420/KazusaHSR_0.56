namespace KazusaHSR.GameServer.Resource;

public class CreateMazeRoom : TaskConfig
{
	public uint RoomID { get; set; }
	public bool IsStartupRoom { get; set; }
	public string RoomUniqueName { get; set; }
	public string AttachAreaName { get; set; }
	public string AttachAnchorName { get; set; }
}
