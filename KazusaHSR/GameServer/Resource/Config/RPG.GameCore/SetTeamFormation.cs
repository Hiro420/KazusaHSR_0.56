namespace KazusaHSR.GameServer.Resource;

public class SetTeamFormation : TaskConfig
{
	public TeamType Team { get; set; }
	public JMHAEAGELCA FormationType { get; set; }
	public string FormationConfigName { get; set; }
	public bool RemoveDying { get; set; }
	public string CustomFormationName { get; set; }
	public TargetEvaluator CustomCenterTargetType { get; set; }
	public bool CustomFormationIgnoreDying { get; set; }
}
