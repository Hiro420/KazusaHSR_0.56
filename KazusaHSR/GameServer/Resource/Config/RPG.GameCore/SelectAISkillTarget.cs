namespace KazusaHSR.GameServer.Resource;

public class SelectAISkillTarget : TaskConfig
{
	public string SkillName { get; set; }
	public bool UseDefault { get; set; }
	public AISelector Selector { get; set; }
}
