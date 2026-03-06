namespace KazusaHSR.GameServer.Resource;

public class ByCompareAlertValue : PredicateConfig
{
	public uint GroupID { get; set; }
	public uint GroupNpcMonsterID { get; set; }
	public JIIEIMBHPNF CompareType { get; set; }
	public float AlertValueMin { get; set; }
	public float AlertValueMax { get; set; }
}
