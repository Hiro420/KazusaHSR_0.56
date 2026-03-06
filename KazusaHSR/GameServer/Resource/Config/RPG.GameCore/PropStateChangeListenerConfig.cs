namespace KazusaHSR.GameServer.Resource;

public class PropStateChangeListenerConfig : TaskConfig
{
	public uint GroupID { get; set; }
	public uint GroupPropID { get; set; }
	public PropState FromState { get; set; }
	public PropState ToState { get; set; }
	public TaskConfig[] OnChange { get; set; }
}
