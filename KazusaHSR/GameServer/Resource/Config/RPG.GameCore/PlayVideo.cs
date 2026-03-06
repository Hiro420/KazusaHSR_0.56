namespace KazusaHSR.GameServer.Resource;

public class PlayVideo : TaskConfig
{
	public string VideoPath { get; set; }
	public bool CanSkip { get; set; }
	public bool IsLoop { get; set; }
	public string BGMGroup { get; set; }
	public string BGMGroupState { get; set; }
}
