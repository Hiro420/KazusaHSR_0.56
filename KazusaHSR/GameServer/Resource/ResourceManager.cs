//using KazusaHSR.Resource.Excel;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;

namespace KazusaHSR.Resource;

public class ResourceManager
{
	public ResourceLoader loader;
	public List<AvatarRow> AvatarExcel { get; set; } = new();
	public List<AvatarSkillRow> AvatarSkillExcel { get; set; } = new();
	public List<AvatarPromotionRow> AvatarPromotionExcel { get; set; } = new();
	public List<TutorialRow> TutorialExcel { get; set; } = new();
	public List<TutorialGuideRow> TutorialGuideExcel { get; set; } = new();
	public List<MainMissionRow> MainMissionExcel { get; set; } = new();
	public List<MazePlaneRow> MazePlaneExcel { get; set; } = new();
	public List<InteractRow> InteractExcel { get; set; } = new();
	public List<MonsterRow> MonsterExcel { get; set; } = new();
	public List<StageRow> StageExcel { get; set; } = new();
	public List<PlaneEventRow> PlaneEventExcel { get; set; } = new();
	public List<NpcRow> NpcExcel { get; set; } = new();
	public List<MapEntranceRow> MapEntranceExcel { get; set; } = new();
	public List<CocoonRow> CocoonExcel { get; set; } = new();
	public List<MazeSkillRow> MazeSkillExcel { get; set; } = new();
	public List<AdventurePlayerRow> AdventurePlayerExcel { get; set; } = new();

	public Dictionary<uint, Dictionary<uint, LevelFloorInfo>> LevelFloorInfos { get; set; } = new();
	public Dictionary<uint, Dictionary<uint, Dictionary<uint, LevelGroupInfo>>> LevelGroups { get; set; } = new();

	public Dictionary<string, AdventureAbilityConfig> AdventureAbilityConfigs { get; set; } = new();
	public Dictionary<string, AdventureCharacterConfig> LocalPlayerConfigs { get; set; } = new();

	// :3
	public ResourceManager(string baseResourcePath = "resources")
	{
		Logger c = new("ResourceLoader");
		c.LogInfo("Loading Resources, this may take a while..");
		this.loader = new(this, baseResourcePath);
		c.LogSuccess("Loaded Resources");
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
}
