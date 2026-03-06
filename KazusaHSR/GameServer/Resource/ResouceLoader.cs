using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.Resource;

public class ResourceLoader
{
	public static readonly string ExcelSubPath = "ExcelOutput";
	public static readonly string JsonSubPath = "Config";
	public string _baseResourcePath;
	private ResourceManager _resourceManager;
	private static Logger logger = new("ResourceLoader");
	private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
	{
		Converters = new List<JsonConverter>
		{
			new TaskConfigJsonConverter()
		}
	};

	public ResourceLoader(ResourceManager resourceManager, string baseResourcePath)
	{
		_baseResourcePath = baseResourcePath;
		this._resourceManager = resourceManager;

		logger.LogInfo("Loading excels/configs, this may take a while...");

		// Load excels/configs here
		this._resourceManager.AvatarExcel = LoadExcel<AvatarRow>("AvatarConfig");
		this._resourceManager.AvatarSkillExcel = LoadExcel<AvatarSkillRow>("AvatarSkillConfig");
		this._resourceManager.AvatarPromotionExcel = LoadExcel<AvatarPromotionRow>("AvatarPromotionConfig");
		this._resourceManager.TutorialExcel = LoadExcel<TutorialRow>("TutorialData");
		this._resourceManager.TutorialGuideExcel = LoadExcel<TutorialGuideRow>("TutorialGuideData");
		this._resourceManager.MainMissionExcel = LoadExcel<MainMissionRow>("MainMission");
		this._resourceManager.MazePlaneExcel = LoadExcel<MazePlaneRow>("MazePlane");
		this._resourceManager.InteractExcel = LoadExcel<InteractRow>("InteractConfig");
		this._resourceManager.MonsterExcel = LoadExcel<MonsterRow>("MonsterConfig");
		this._resourceManager.StageExcel = LoadExcel<StageRow>("StageConfig");
		this._resourceManager.PlaneEventExcel = LoadExcel<PlaneEventRow>("PlaneEvent");
		this._resourceManager.NpcExcel = LoadExcel<NpcRow>("NpcData");
		this._resourceManager.MapEntranceExcel = LoadExcel<MapEntranceRow>("MapEntrance");
		this._resourceManager.CocoonExcel = LoadExcel<CocoonRow>("CocoonConfig");
		this._resourceManager.MazeSkillExcel = LoadExcel<MazeSkillRow>("MazeSkill");
		this._resourceManager.AdventurePlayerExcel = LoadExcel<AdventurePlayerRow>("AdventurePlayer");
		logger.LogSuccess("Finished loading excels");

		// Load Configs
		foreach (MazePlaneRow row in this._resourceManager.MazePlaneExcel)
		{
			uint planeId = row.PlaneId;
			foreach (uint floorId in row.FloorIdList)
			{
				string fileName = $"P{planeId}_F{floorId}";
				LevelFloorInfo floorInfo = LoadConfig<LevelFloorInfo>(fileName, "LevelOutput", "Floor");
				if (!this._resourceManager.LevelFloorInfos.ContainsKey(planeId))
				{
					this._resourceManager.LevelFloorInfos[planeId] = new Dictionary<uint, LevelFloorInfo>();
				}
				this._resourceManager.LevelFloorInfos[planeId][floorId] = floorInfo;

				if (!this._resourceManager.LevelGroups.ContainsKey(planeId))
				{
					this._resourceManager.LevelGroups[planeId] = new Dictionary<uint, Dictionary<uint, LevelGroupInfo>>();
				}
				this._resourceManager.LevelGroups[planeId][floorId] = new Dictionary<uint, LevelGroupInfo>();

				foreach (var group in floorInfo.GroupList)
				{
					uint groupId = group.ID;
					LevelGroupInfo groupInfo = LoadConfig<LevelGroupInfo>(
						$"LevelGroup_P{planeId}_F{floorId}_G{groupId}", 
						"LevelOutput", 
						"Group",
						$"Groups_P{planeId}_F{floorId}"
					);
					this._resourceManager.LevelGroups[planeId][floorId][groupId] = groupInfo;
				}
			}
		}

		// recursively load all adventure ability configs from Config\ConfigAdventureAbility
		string root = System.IO.Path.Combine(_baseResourcePath, JsonSubPath);
		string adventureAbilityDir = System.IO.Path.Combine(root, "ConfigAdventureAbility");

		var configFiles_adventureAbility = System.IO.Directory.GetFiles(
			adventureAbilityDir,
			"*.json",
			System.IO.SearchOption.AllDirectories
		);

		foreach (var filePath in configFiles_adventureAbility)
		{
			string rel = System.IO.Path.GetRelativePath(root, filePath);
			string fileName = System.IO.Path.GetFileNameWithoutExtension(rel);
			string? relDir = System.IO.Path.GetDirectoryName(rel);

			string[] extraPaths = string.IsNullOrEmpty(relDir)
				? Array.Empty<string>()
				: relDir.Split(System.IO.Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

			var config = LoadConfig<AdventureAbilityConfigList>(fileName, extraPaths);

			foreach (var ability in config.AbilityList ?? Enumerable.Empty<AdventureAbilityConfig>())
			{
				_resourceManager.AdventureAbilityConfigs[ability.Name] = ability;
			}
		}


		// recursively load all adventure character configs from Config\ConfigCharacter\LocalPlayer
		string localPlayerConfigPath = System.IO.Path.Combine(_baseResourcePath, JsonSubPath, "ConfigCharacter", "LocalPlayer");
		string localPlayerDir = System.IO.Path.Combine(root, "ConfigCharacter", "LocalPlayer");
		var localPlayerConfigfiles = System.IO.Directory.GetFiles(localPlayerDir, "*.json", System.IO.SearchOption.AllDirectories);

		foreach (var filePath in localPlayerConfigfiles)
		{
			string rel = System.IO.Path.GetRelativePath(root, filePath);
			string fileName = System.IO.Path.GetFileNameWithoutExtension(rel);
			string? relDir = System.IO.Path.GetDirectoryName(rel);
			string[] extraPaths = string.IsNullOrEmpty(relDir)
				? Array.Empty<string>()
				: relDir.Split(System.IO.Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

			AdventureCharacterConfig config = LoadConfig<AdventureCharacterConfig>(fileName, extraPaths);

			// key should be forward slashes!
			string key = Path.Combine(JsonSubPath, rel).Replace(System.IO.Path.DirectorySeparatorChar, '/');
			_resourceManager.LocalPlayerConfigs[key] = config;
		}

		logger.LogSuccess("Finished loading configs");
	}

	private List<T> LoadExcel<T>(string fileName)
	{
		string fullPath = System.IO.Path.Combine(_baseResourcePath, ExcelSubPath, $"{fileName}.json");
		string json = System.IO.File.ReadAllText(fullPath);
		List<T>? rows = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
		if (rows == null)
		{
			logger.LogError($"Failed to load Excel: {fullPath}");
			return new List<T>();
		}
		//logger.LogSuccess($"Loaded Excel: {fullPath} with {rows.Count} rows");
		return rows;
	}

	private T LoadConfig<T>(string fileName, params string[] extraPaths)
	{
		string fullPath = System.IO.Path.Combine(_baseResourcePath, JsonSubPath, System.IO.Path.Combine(extraPaths), $"{fileName}.json");
		string json = System.IO.File.ReadAllText(fullPath);
		T? config = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
		if (config == null)
		{
			logger.LogError($"Failed to load Config: {fullPath}");
			throw new Exception($"Failed to load Config: {fullPath}");
		}
		//logger.LogSuccess($"Loaded Config: {fullPath}");
		return config;
	}
}

public class TaskConfigJsonConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
		=> objectType == typeof(TaskConfig);

	public override object? ReadJson(
		JsonReader reader,
		Type objectType,
		object? existingValue,
		JsonSerializer serializer)
	{
		var jo = JObject.Load(reader);

		var typeName = (string?)jo["$type"];
		if (string.IsNullOrEmpty(typeName))
			return null;

		var concreteType = Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.FirstOrDefault(t =>
				typeof(TaskConfig).IsAssignableFrom(t) &&
				!t.IsAbstract &&
				t.Name == typeName.Split("RPG.GameCore.").Last());

		if (concreteType == null)
			return null;

		return jo.ToObject(concreteType, serializer);
	}

	public override void WriteJson(
		JsonWriter writer,
		object? value,
		JsonSerializer serializer)
	{
		JObject.FromObject(value!, serializer).WriteTo(writer);
	}
}
