namespace KazusaHSR.GameServer.Resource;

public class DestroyNPCMonster : TaskConfig
{
	public uint GroupID { get; set; }
	public uint[] GroupMonsterIDs { get; set; }
	public string[] UniqueNames { get; set; }
}
