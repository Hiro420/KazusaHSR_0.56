namespace KazusaHSR.GameServer.Resource;

public class ByCompareCharacterID : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public int TargetCharacterID { get; set; }
}
