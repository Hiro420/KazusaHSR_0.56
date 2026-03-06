namespace KazusaHSR.GameServer.Resource;

public class SetNpcStatus : TaskConfig
{
	public uint GroupID { get; set; }
	public uint GroupNpcID { get; set; }
	public DNKGGMEMJPL Status { get; set; }
}
