namespace KazusaHSR.GameServer.Resource;

public class EnableNPCMonsterAI : TaskConfig
{
	public bool Enable { get; set; }
	public uint GroupID { get; set; }
	public uint[] GroupMonsterIDs { get; set; }
	public string[] UniqueNames { get; set; }
	public bool AbortSkillWhenDisable { get; set; }
}
