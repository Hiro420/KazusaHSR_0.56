namespace KazusaHSR.GameServer.Resource;

public class AlignTargetToTeamCenter : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool AlignFaceDir { get; set; }
	public bool UseNormalFormation { get; set; }
}
