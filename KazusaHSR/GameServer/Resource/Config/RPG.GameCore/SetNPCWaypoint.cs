namespace KazusaHSR.GameServer.Resource;

public class SetNPCWaypoint : TaskConfig
{
	public uint GroupInstanceID { get; set; }
	public WaypointConfig[] WaypointCfgs { get; set; }
	public TaskConfig[] BehaviourList { get; set; }
}
