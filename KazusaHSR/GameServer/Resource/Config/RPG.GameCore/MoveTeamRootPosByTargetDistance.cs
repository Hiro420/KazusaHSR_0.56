namespace KazusaHSR.GameServer.Resource;

public class MoveTeamRootPosByTargetDistance : TaskConfig
{
	public bool MoveTarget { get; set; }
	public bool Reset { get; set; }
	public FormationTargetDistanceConfig[] DistanceConfigList { get; set; }
}
