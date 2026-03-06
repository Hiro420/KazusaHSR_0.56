namespace KazusaHSR.GameServer.Resource;

public class DestroyNPC : TaskConfig
{
	public uint NpcID { get; set; }
	public uint GroupID { get; set; }
	public uint GroupNpcID { get; set; }
}
