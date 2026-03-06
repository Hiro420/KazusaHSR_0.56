namespace KazusaHSR.GameServer.Resource;

public class UseSkill : TaskConfig
{
	public string SkillName { get; set; }
	public IJGJIJAJAIE ControlSkillType { get; set; }
	public bool RecoverySkillCD { get; set; }
}
