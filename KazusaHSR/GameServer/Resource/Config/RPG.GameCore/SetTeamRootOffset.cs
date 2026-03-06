namespace KazusaHSR.GameServer.Resource;

public class SetTeamRootOffset : TaskConfig
{
	public TeamType Team { get; set; }
	public bool Reset { get; set; }
	public MVector3 Offset { get; set; }
}
