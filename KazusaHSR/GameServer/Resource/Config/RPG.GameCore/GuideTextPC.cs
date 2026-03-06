namespace KazusaHSR.GameServer.Resource;

public class GuideTextPC
{
	public bool UsePCGuide { get; set; }
	public bool Skip { get; set; }
	public int GuideResID { get; set; }
	public string Path { get; set; }
	public string TextPath { get; set; }
	public string Text { get; set; }
	public string ActionName { get; set; }
	public bool CopyAnchorAndSale { get; set; }
	public float OffsetX { get; set; }
	public float OffsetY { get; set; }
}
