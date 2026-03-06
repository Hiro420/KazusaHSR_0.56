namespace KazusaHSR.GameServer.Resource;

public class SaveCheckPoint : TaskConfig
{
	public DynamicFloat GroupID { get; set; }
	public DynamicFloat GroupAnchorID { get; set; }
	public DynamicFloat HPRatio { get; set; }
	public DynamicFloat SPRatio { get; set; }
}
