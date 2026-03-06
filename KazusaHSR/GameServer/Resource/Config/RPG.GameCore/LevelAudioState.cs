namespace KazusaHSR.GameServer.Resource;

public class LevelAudioState : TaskConfig
{
	public string GroupName { get; set; }
	public DynamicString StateName { get; set; }
	public bool SaveToCustomString { get; set; }
}
