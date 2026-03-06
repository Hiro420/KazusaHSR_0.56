namespace KazusaHSR.GameServer.Resource;

public class AIConfigInfo
{
	public string AIFile { get; set; }
	public AIPathwayInfo[] PathwayList { get; set; }
	public uint DefaultAIPathwayIndex { get; set; }
}
