namespace KazusaHSR.GameServer.Resource;

public class ByCompareCharacterNumber : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public PredicateConfig Predicate { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public uint CompareNumber { get; set; }
}
