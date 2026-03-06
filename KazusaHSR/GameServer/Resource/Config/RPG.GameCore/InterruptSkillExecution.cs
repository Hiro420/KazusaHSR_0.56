namespace KazusaHSR.GameServer.Resource;

public class InterruptSkillExecution : TaskConfig
{
	public TaskConfig[] OnSucceed { get; set; }
	public TaskConfig[] OnFailure { get; set; }
}
