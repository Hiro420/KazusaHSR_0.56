namespace KazusaHSR.GameServer.Resource;

public class ByCompareMonsterRank : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public uint CompareValue { get; set; }
}
