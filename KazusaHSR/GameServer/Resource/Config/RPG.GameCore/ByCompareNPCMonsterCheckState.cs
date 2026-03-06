namespace KazusaHSR.GameServer.Resource;

public class ByCompareNPCMonsterCheckState : PredicateConfig
{
	public string UniqueName { get; set; }
	public uint GroupID { get; set; }
	public uint GroupMonsterID { get; set; }
	public JNPLGHCCPPD CheckState { get; set; }
}
