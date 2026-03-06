namespace KazusaHSR.GameServer.Resource;

public class ShowGuide : TaskConfig
{
	public int GuideResID { get; set; }
	public bool Show { get; set; }
	public string Path { get; set; }
	public bool CopyAnchorAndSale { get; set; }
	public float OffsetX { get; set; }
	public float OffsetY { get; set; }
	public GuidePC PCGuide { get; set; }
}
