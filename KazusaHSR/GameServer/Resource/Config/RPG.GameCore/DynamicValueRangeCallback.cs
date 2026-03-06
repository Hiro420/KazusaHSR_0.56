namespace KazusaHSR.GameServer.Resource;

public class DynamicValueRangeCallback
{
	public DynamicFloat Min { get; set; }
	public DynamicFloat Max { get; set; }
	public bool MaxInclusive { get; set; }
	public TaskConfig[] OnEnterRange { get; set; }
	public TaskConfig[] OnExitRange { get; set; }
}
