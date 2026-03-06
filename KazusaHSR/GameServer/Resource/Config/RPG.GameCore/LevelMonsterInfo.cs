namespace KazusaHSR.GameServer.Resource;

public class LevelMonsterInfo : NamedLevelObjectInfo
{
	public uint NPCMonsterID { get; set; }
	public string LevelGraph { get; set; }
	public AIConfigInfo AIConfig { get; set; }
	public bool CreateOnInitial { get; set; } = true;
	public LevelGraphValueSource ValueSource { get; set; }
	public uint CampID { get; set; }
	public BattleAreaReferenceInfo BattleArea { get; set; }
	public LevelTriggerInfo Trigger { get; set; }
	public LevelAnimatingObjectState InitialAnimState { get; set; }
	public uint RecordID { get; set; }
	public uint EventID { get; set; }
}
