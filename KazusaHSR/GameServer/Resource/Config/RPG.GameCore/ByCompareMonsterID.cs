namespace KazusaHSR.GameServer.Resource;

public class ByCompareMonsterID : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public int TargetMonsterID { get; set; }
}
