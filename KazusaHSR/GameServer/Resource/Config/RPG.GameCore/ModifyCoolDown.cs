namespace KazusaHSR.GameServer.Resource;

public class ModifyCoolDown : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public int SkillIndex { get; set; }
	public IGNHFEOEBHO ModifyFunction { get; set; }
	public int ModifyValue { get; set; }
}
