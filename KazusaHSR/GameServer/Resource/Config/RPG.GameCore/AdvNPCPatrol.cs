namespace KazusaHSR.GameServer.Resource;

public class AdvNPCPatrol : TaskConfig
{
	public bool StayOnWaypoint { get; set; }
	public float StayOnWaypointDuration { get; set; }
	public TaskConfig OnWaypointTask { get; set; }
}
