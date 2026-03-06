namespace KazusaHSR.GameServer.Resource;

public class RefreshTeamFormationWhenNextCameraShot : TaskConfig
{
	public TeamType Team { get; set; }
	public JMHAEAGELCA FormationType { get; set; }
	public bool ResetFormationRootPos { get; set; }
}
