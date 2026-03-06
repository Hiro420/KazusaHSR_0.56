namespace KazusaHSR.GameServer.Resource;

public class LevelStoryInitialize : TaskConfig
{
	public string AreaPrefabPath { get; set; }
	public bool LockPlayControl { get; set; }
	public bool LockCamera { get; set; }
	public EntityVisiableInfo[] EntityVisiableList { get; set; }
	public CreateCharacter[] CreateCharacterList { get; set; }
	public bool CloseFootIK { get; set; }
	public bool NeedShowTalkUI { get; set; }
	public bool HideBillboard { get; set; }
	public bool HideEffect { get; set; }
	public bool HideProp { get; set; }
	public bool EnableRootMotion { get; set; }
}
