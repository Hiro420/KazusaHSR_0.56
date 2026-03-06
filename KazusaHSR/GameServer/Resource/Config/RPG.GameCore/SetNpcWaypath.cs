namespace KazusaHSR.GameServer.Resource;

public class SetNpcWaypath : TaskConfig
{
	public BHFEGIFCBOE UsageType { get; set; }
	public uint WaypathIdx { get; set; }
	public uint GroupID { get; set; }
	public uint GroupNpcID { get; set; }
}
