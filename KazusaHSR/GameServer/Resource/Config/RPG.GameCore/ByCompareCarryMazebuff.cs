namespace KazusaHSR.GameServer.Resource;

public class ByCompareCarryMazebuff : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public uint BuffID { get; set; }
}
