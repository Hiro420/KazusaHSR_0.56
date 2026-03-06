namespace KazusaHSR.GameServer.Resource;

public class ResetStance : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat ConstantValue { get; set; }
	public DynamicFloat AddValue { get; set; }
	public bool ForbidWhenEmpty { get; set; }
	public bool SkipLockTeamStance { get; set; }
}
