namespace KazusaHSR.GameServer.Resource;

public class CreateNPC : TaskConfig
{
	public uint GroupID { get; set; }
	public uint GroupNpcID { get; set; }
	public string NPCUniqueName { get; set; }
	public string DefaultIdleStateName { get; set; }
}
