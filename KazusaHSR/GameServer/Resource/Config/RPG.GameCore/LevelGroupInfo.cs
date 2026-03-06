namespace KazusaHSR.GameServer.Resource;

public class LevelGroupInfo
{
	public string GroupGUID { get; set; }
	public string GroupName { get; set; }
	public string ConfigPrefabPath { get; set; }
	public string LevelGraph { get; set; }
	public string Model { get; set; }
	public string AreaAnchorName { get; set; }
	public MKEHACCAIMP SaveType { get; set; }
	public uint CheckClearMainMissionID { get; set; }
	public uint UnlockMainMissionID { get; set; }
	public LevelAnchorInfo[] AnchorList { get; set; }
	public LevelModelInfo[] ModelList { get; set; }
	public LevelMonsterInfo[] MonsterList { get; set; }
	public LevelPropInfo[] PropList { get; set; }
	public LevelWaypointInfo[] WaypointList { get; set; }
	public LevelPathwayInfo[] PathwayList { get; set; }
	public LevelBattleAreaInfo[] BattleAreaList { get; set; }
	public LevelNPCInfo[] NPCList { get; set; }
	public uint GroupRefreshID { get; set; }
	public RandomNPCMonsterInfo[] RandomNPCMonsterList { get; set; }
	public uint[] InitialRandomNPCMonsterIDList { get; set; }
}
