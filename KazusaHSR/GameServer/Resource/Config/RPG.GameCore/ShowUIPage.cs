namespace KazusaHSR.GameServer.Resource;

public class ShowUIPage : TaskConfig
{
	public string AssetPath { get; set; }
	public bool WaitShowPageFinish { get; set; }
	public UIImageNode[] UIImageNodes { get; set; }
	public UITextNode[] UITextNodes { get; set; }
	public float LifeTime { get; set; }
}
