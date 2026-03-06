namespace KazusaHSR.GameServer.Resource;

public class SetControlSkillMapping : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public IJGJIJAJAIE ControlSkillType { get; set; }
	public string SkillTriggerKey { get; set; }
}
