namespace KazusaHSR.GameServer.Resource;

public class ByCompareNpcMonsterRank : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public uint CompareValue { get; set; }
}
