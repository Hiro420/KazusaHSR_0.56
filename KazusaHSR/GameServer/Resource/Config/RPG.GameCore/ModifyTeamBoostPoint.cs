namespace KazusaHSR.GameServer.Resource;

public class ModifyTeamBoostPoint : TaskConfig
{
	public TeamType Team { get; set; }
	public IGNHFEOEBHO ModifyFunction { get; set; }
	public DynamicFloat ModifyValue { get; set; }
}
