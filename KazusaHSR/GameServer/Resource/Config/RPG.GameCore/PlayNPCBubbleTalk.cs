namespace KazusaHSR.GameServer.Resource;

public class PlayNPCBubbleTalk : TaskConfig
{
	public BubbleTalkInfo[] BubbleTalkInfoList { get; set; }
	public bool InstantFinish { get; set; }
	public uint ID { get; set; }
}
