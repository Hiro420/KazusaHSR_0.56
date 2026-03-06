namespace KazusaHSR.GameServer.Resource;

public class AdvNPCChase : TaskConfig
{
	public float ChaseRadius { get; set; }
	public float SearchOffsetFromTarget { get; set; }
	public TaskConfig OnMissTarget { get; set; }
	public TaskConfig OnFoundTarget { get; set; }
	public TaskConfig OnEnterChase { get; set; }
	public TaskConfig OnLeaveChase { get; set; }
}
