namespace KazusaHSR.GameServer.Resource;

public class CreateNPCMonster : TaskConfig
{
	public bool AttachEnterBattleTrigger { get; set; }
	public bool AttachTalkTrigger { get; set; }
	public uint GroupID { get; set; }
	public uint GroupNpcMonsterID { get; set; }
	public uint NPCMonsterID { get; set; }
	public string NPCMonsterUniqueName { get; set; }
	public string DefaultIdleStateName { get; set; }
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
	public JNCHJAFHMMI CampID { get; set; }
	public TaskConfig[] OnCreate { get; set; }
	public TaskConfig[] OnDie { get; set; }
}
