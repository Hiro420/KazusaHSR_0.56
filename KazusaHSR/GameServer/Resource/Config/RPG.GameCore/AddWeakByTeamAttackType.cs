namespace KazusaHSR.GameServer.Resource;

public class AddWeakByTeamAttackType : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat ResistanceDeltaValue { get; set; }
	public DynamicFloat AddWeakCountMax { get; set; }
}
