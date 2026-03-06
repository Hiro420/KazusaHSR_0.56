namespace KazusaHSR.GameServer.Resource;

public class ModifySkillDataBpProperty : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat BpAdd { get; set; }
	public DynamicFloat BpNeed { get; set; }
}
