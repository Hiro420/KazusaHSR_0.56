//using KazusaHSR.Resource.Excel;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;

namespace KazusaHSR.Resource;

public class ResourceManager
{
	public ResourceLoader loader;
	public List<AvatarRow> AvatarExcel { get; set; } = new();
	public List<AvatarSkillRow> AvatarSkillExcel { get; set; } = new();
	public List<AvatarSkillTreeRow> AvatarSkillTreeExcel { get; set; } = new();
	public List<AvatarPromotionRow> AvatarPromotionExcel { get; set; } = new();
	public List<TutorialRow> TutorialExcel { get; set; } = new();
	public List<TutorialGuideRow> TutorialGuideExcel { get; set; } = new();
	public List<MainMissionRow> MainMissionExcel { get; set; } = new();
	public List<MazePlaneRow> MazePlaneExcel { get; set; } = new();
	public List<InteractRow> InteractExcel { get; set; } = new();
	public List<ItemRow> ItemConfig { get; set; } = new();
	public List<ItemRow> ItemConfigAvatar { get; set; } = new();
	public List<ItemRow> ItemConfigEquipment { get; set; } = new();
	public List<MonsterRow> MonsterExcel { get; set; } = new();
	public List<StageRow> StageExcel { get; set; } = new();
	public List<ShopConfigRow> ShopConfig { get; set; } = new();
	public List<ShopGoodsConfigRow> ShopGoodsConfig { get; set; } = new();
	public List<ShopGoodsGroupConfigRow> ShopGoodsGroupConfig { get; set; } = new();
	public List<PlaneEventRow> PlaneEventExcel { get; set; } = new();
	public List<NpcRow> NpcExcel { get; set; } = new();
	public List<MapEntryRow> MapEntranceExcel { get; set; } = new();
	public List<CocoonRow> CocoonExcel { get; set; } = new();
	public List<MazeSkillRow> MazeSkillExcel { get; set; } = new();
	public List<AdventurePlayerRow> AdventurePlayerExcel { get; set; } = new();
	public List<ChallengeMazeConfigRow> ChallengeMazeExcel { get; set; } = new();
	public List<ChallengeTargetConfigRow> ChallengeTargetConfig { get; set; } = new();
	public List<EquipmentExpItemRow> EquipmentExpItemConfig { get; set; } = new();
	public List<EquipmentRow> EquipmentConfig { get; set; } = new();
	public List<EquipmentExpTypeRow> EquipmentExpType { get; set; } = new();
	public List<QuestDataRow> QuestData { get; set; } = new();
	public List<WorldLevelRow> WorldLevelConfig { get; set; } = new();
	public List<FinishWayRow> FinishWay { get; set; } = new();
	public List<DailyMissionRewardRow> DailyMissionReward { get; set; } = new();
	public List<DailyMissionRandomDataRow> DailyMissionRandomData { get; set; } = new();
	public List<MazeBuffRow> MazeBuff { get; set; } = new();
	public List<EquipmentPromotionRow> EquipmentPromotionConfig { get; set; } = new();

	public Dictionary<uint, Dictionary<uint, LevelFloorInfo>> LevelFloorInfos { get; set; } = new();
	public Dictionary<uint, Dictionary<uint, Dictionary<uint, LevelGroupInfo>>> LevelGroups { get; set; } = new();

	public Dictionary<string, AdventureAbilityConfig> AdventureAbilityConfigs { get; set; } = new();
	public Dictionary<string, AdventureCharacterConfig> LocalPlayerConfigs { get; set; } = new();

	public Dictionary<long, TextmapRow> Textmap_en { get; set; } = new();
	public Dictionary<long, TextmapRow> Textmap_cn { get; set; } = new();

	// :3
	public ResourceManager(string baseResourcePath = "resources")
	{
		Logger c = new("ResourceLoader");
		c.Message("Loading Resources, this may take a while..");
		this.loader = new(this, baseResourcePath);
		c.Emit("Loaded Resources");
	}

	public AdventureAbilityConfig? GetAdventureAbilityConfig(string abilityName)
	{
		if (AdventureAbilityConfigs.TryGetValue(abilityName, out AdventureAbilityConfig? config))
		{
			return config;
		}
		return null;
	}

	public AdventureCharacterConfig? GetLocalPlayerConfig(string jsonPath)
	{
		if (LocalPlayerConfigs.TryGetValue(jsonPath, out AdventureCharacterConfig? config))
		{
			return config;
		}
		return null;
	}

	public ItemRow? GetItemRowById(uint itemId)
	{
		ItemRow? row = ItemConfig.FirstOrDefault(i => i.ID == itemId);
		if (row == null)
			row = ItemConfigAvatar.FirstOrDefault(i => i.ID == itemId);
		if (row == null)
			row = ItemConfigEquipment.FirstOrDefault(i => i.ID == itemId);
		return row;
	}
}
