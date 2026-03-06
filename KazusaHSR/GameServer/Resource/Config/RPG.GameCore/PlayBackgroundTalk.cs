namespace KazusaHSR.GameServer.Resource;

public class PlayBackgroundTalk : TaskConfig
{
	public BackgroundTalkInfo[] BackgroundTalkList { get; set; }
	public bool InstantFinish { get; set; }
}
