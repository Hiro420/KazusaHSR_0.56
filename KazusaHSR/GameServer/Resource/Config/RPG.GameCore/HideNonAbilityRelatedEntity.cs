namespace KazusaHSR.GameServer.Resource;

public class HideNonAbilityRelatedEntity : TaskConfig
{
	public TargetEvaluator ExclusiveTargetType { get; set; }
	public bool IsHide { get; set; }
}
