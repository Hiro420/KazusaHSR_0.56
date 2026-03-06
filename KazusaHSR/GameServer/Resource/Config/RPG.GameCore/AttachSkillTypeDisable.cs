namespace KazusaHSR.GameServer.Resource;

public class AttachSkillTypeDisable : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public SkillType[] Types { get; set; }
}
