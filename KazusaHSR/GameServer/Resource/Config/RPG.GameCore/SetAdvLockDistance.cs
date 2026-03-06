namespace KazusaHSR.GameServer.Resource;

public class SetAdvLockDistance : TaskConfig
{
	public AdventureSkillType SkillType { get; set; }
	public bool IsPushValue { get; set; }
	public float LockDistance { get; set; }
}
