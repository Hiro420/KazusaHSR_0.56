namespace KazusaHSR.GameServer.Resource;

public class ByCompareTargetNatureID : PredicateConfig
{
	public TargetEvaluator TargetType { get; set; }
	public uint TargetNatureID { get; set; }
}
