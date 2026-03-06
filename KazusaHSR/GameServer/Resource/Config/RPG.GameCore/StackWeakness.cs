namespace KazusaHSR.GameServer.Resource;

public class StackWeakness : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public PFMBLINOLCP OPType { get; set; }
	public AttackDamageType[] WeakList { get; set; }
}
