namespace KazusaHSR.GameServer.Resource;

public class SetNPCMonsterWaypoint : TaskConfig
{
	public string UniqueName { get; set; }
	public string WaypointAreaName { get; set; }
	public DeprecatedWaypointConfig[] WaypointCfgs { get; set; }
	public TaskConfig[] BehaviourList { get; set; }
}
