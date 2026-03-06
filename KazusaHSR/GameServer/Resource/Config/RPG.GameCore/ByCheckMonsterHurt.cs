namespace KazusaHSR.GameServer.Resource;

public class ByCheckMonsterHurt : PredicateConfig
{
	public uint MonsterID { get; set; }
	public float HpPercent { get; set; }
	public BKPBIOHOFJB OverrideConfig { get; set; }
}
