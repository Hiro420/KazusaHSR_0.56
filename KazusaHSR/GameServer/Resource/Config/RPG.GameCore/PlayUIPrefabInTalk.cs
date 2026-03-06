namespace KazusaHSR.GameServer.Resource;

public class PlayUIPrefabInTalk : TaskConfig
{
	public float AutoSkipTime { get; set; }
	public string AssetPath { get; set; }
	public UIImageNode[] UIImageNodes { get; set; }
	public UITextNode[] UITextNodes { get; set; }
}
