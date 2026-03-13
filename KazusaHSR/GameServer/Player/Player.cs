using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public class Player
{
	private Session session { get; set; }
	public Session Session => session;
	private Logger logger = new("Player");
	public string Name { get; set; }
	public uint Level { get; set; }
	public uint WorldLevel => ResolveWorldLevel(Level);
	public uint Uid { get; set; }
	public HashSet<uint> DailyMissionIds { get; set; } = new();
	public Dictionary<ulong, PlayerAvatar> avatarDict { get; set; } = new();
	//public Dictionary<ulong, PlayerWeapon> weaponDict { get; set; }
	public Dictionary<uint, PlayerItem> itemDict { get; set; } = new();
	public uint LastItemGuid { get; set; }
	public Protocol.Vector Pos { get; private set; }
	public Protocol.Vector Rot { get; private set; } // wont actually be used except for scene tp
	public List<PlayerTeam> teamList { get; set; } = new(4);
	// todo
	//public PlayerTeam ExtraLineup { get; set; } = new PlayerTeam(null!); // for virtual avatars and such, not persisted
	public uint TeamIndex { get; set; } = 0;
	public Scene Scene { get; set; }
	public uint Exp { get; set; }
	public BattleManager battleManager { get; set; }
	public TeamManager TeamManager { get; }
	public ItemManager ItemManager { get; }
	public ShopManager ShopManager { get; set; }
	public ChallengeManager ChallengeManager { get; }

	public Player(Session session, uint uid)
	{
		//WorldLevelRow maxRow = MainApp.resourceManager.WorldLevelConfig.OrderByDescending(row => row.Level).First();
		this.session = session;
		this.Uid = uid;
		this.Name = "KazusaHSR";
		this.Level = 1;
		this.Exp = 0;
		this.Scene = new Scene(session, 10101, 10101001, 0, false); // LevelGroup_P10101_F10101001_G1
		(this.Pos, this.Rot) = this.Scene.GetDefaultSpawnPosAndRot();
		battleManager = new BattleManager(session);
		TeamManager = new TeamManager(this);
		ItemManager = new ItemManager(this);
		ShopManager = new ShopManager(this);
		ChallengeManager = new ChallengeManager(this);
		LastItemGuid = 0; // Initialize LastItemGuid
	}

	public uint GetNextItemGuid()
	{
		LastItemGuid++;
		return LastItemGuid;
	}

	public void SavePersistent()
	{
		try
		{
			// For now we block synchronously; call sites are rare (e.g. logout, team changes).
			MainApp.databaseManager
				.SavePlayerAsync(session.AccountId, session.Token, this)
				.GetAwaiter()
				.GetResult();
		}
		catch (Exception ex)
		{
			logger.Fail($"Failed to persist player {Uid}: {ex.Message}");
		}
	}

	public void SetPosAndRot(Protocol.Vector pos, Protocol.Vector rot)
	{
		this.Pos = pos;
		this.Rot = rot;
	}

	public void AddBasicAvatar()
	{
		uint avatarId = 1007;
		PlayerAvatar playerAvatar = new(session, avatarId);
		for (int i = 0; i < 4; i++)
		{
			// create default team with only trailblazer
			this.teamList[i] = new PlayerTeam(session, playerAvatar);
		}
		this.avatarDict.Add(playerAvatar.Guid, playerAvatar);
	}

	public List<PlayerAvatar> GiveAllAvatars(uint level = 1)
	{
		List<PlayerAvatar> avatars = [];
		foreach (AvatarRow avatarRow in MainApp.resourceManager.AvatarExcel)
		{
			if (avatarRow.AvatarID == 1007)
				continue;
			if (avatarRow.Release == false && !MainApp.config.GameServer.AllowTestCharacters)
				continue;
			if (session.player.avatarDict.Any(a => a.Value.AvatarId == avatarRow.AvatarID))
				continue;
			PlayerAvatar playerAvatar = new(session, avatarRow.AvatarID);
			AvatarPromotionRow? maxPromo = MainApp.resourceManager.AvatarPromotionExcel.Where(p => p.AvatarID == avatarRow.AvatarID)
				.OrderByDescending(p => p.Promotion)
				.FirstOrDefault();
			if (maxPromo != null)
				playerAvatar.Level = Math.Min(maxPromo.MaxLevel, level);
			else
				playerAvatar.Level = Math.Min(80, level);
			playerAvatar.PromoteLevel = ResolvePromotionAV(avatarRow, playerAvatar.Level);
			avatars.Add(playerAvatar);
			this.avatarDict.Add(playerAvatar.Guid, playerAvatar);
			if (this.GetCurrentLineup().Avatars.Count(a => a != null) < 4)
				this.GetCurrentLineup().AddAvatar(session, playerAvatar);
		}
		return avatars;
	}

	private uint ResolvePromotionAV(AvatarRow avatarRow, uint level)
	{
		IEnumerable<AvatarPromotionRow> promotionRows = MainApp.resourceManager.AvatarPromotionExcel.Where(i =>
			i.MaxLevel >= level && i.AvatarID == avatarRow.AvatarID);
		return promotionRows.OrderByDescending(i => i.Promotion).Select(i => i.Promotion).FirstOrDefault();
	}

	public List<PlayerItem> GiveAllItems(uint level = 1)
	{
		List<PlayerItem> items = [];

		// Add normal items, not including equipment for now
		foreach (ItemRow itemRow in MainApp.resourceManager.ItemConfig)
		{
			if (itemRow.ID == 0)
				continue;

			PlayerItem? item = null;

			if (session.player.itemDict.Values.Any(i => i.ItemId == itemRow.ID))
			{
				// increase the amount instead
				item = session.player.itemDict.Values.First(i => i.ItemId == itemRow.ID);
				item.Count = itemRow.PileLimit;
			}
			else
			{
				switch (itemRow.ItemType)
				{
					case Resource.Excel.ItemType.Material:
					case Resource.Excel.ItemType.Gift:
					case Resource.Excel.ItemType.Mission:
					case Resource.Excel.ItemType.Book:
					case Resource.Excel.ItemType.Food:
						item = new(session, itemRow.ID);
						item.Count = Math.Max(itemRow.PileLimit, 100);
						this.itemDict.Add(item.Guid, item);
						break;
					case Resource.Excel.ItemType.Virtual:
						item = new(session, itemRow.ID);
						item.Count = Math.Max(itemRow.PileLimit, 999);
						this.itemDict.Add(item.Guid, item);
						break;
					default:
						// skip other item types for now
						break;
				}
			}
			if (item != null)
			{
				items.Add(item);
			}
		}

		foreach (ItemRow itemRow in MainApp.resourceManager.ItemConfigEquipment)
		{
			if (itemRow.ID == 0)
				continue;

			ItemLightcone equipItem = new(session, itemRow.ID);
			equipItem.Count = 1; // equipment items are not stackable
			equipItem.Level = Math.Max(equipItem.GetMaxLevel(), level);
			items.Add(equipItem);
			this.itemDict.Add(equipItem.Guid, equipItem);
		}

		return items;
	}

	public void InitTeams()
	{
		TeamManager.Clear();
		for (int i = 0; i < 4; i++)
		{
			TeamManager.AddTeam(new PlayerTeam(session));
		}
	}

	public AvatarEntity? FindEntityByPlayerAvatar(PlayerAvatar playerAvatar)
	{
		List<AvatarEntity> avatarEntities = this.Scene.EntityManager.Entities.Values
			.OfType<AvatarEntity>()
			.ToList();
		return avatarEntities.FirstOrDefault(c => c.DbInfo.AvatarId == playerAvatar.AvatarId);
	}

	public void SendSyncLineupNotify()
	{
		session.SendPacket(new SyncLineupNotify()
		{
			Lineup = this.GetCurrentLineup().ToTeamProto(),
		});
	}

	public void SetRot(Protocol.Vector rot)
	{
		this.Rot = rot;
	}

	public void SetPos(Protocol.Vector pos)
	{
		this.Pos = pos;
	}

	public PlayerTeam GetCurrentLineup()
	{
		return TeamManager.GetTeamByIndex((int)this.TeamIndex);
	}

	public uint GetLeaderEntityId()
	{
		PlayerTeam lineup = GetCurrentLineup();
		AvatarEntity? leaderEntity = FindEntityByPlayerAvatar(lineup.Leader!);
		if (leaderEntity == null)
		{
			logger.Fail("Leader entity not found in scene");
			return 0;
		}
		return leaderEntity._EntityId;
	}

	public void ResetPosToLastEntrance()
	{
		PropEntity? entranceProp = this.Scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(prop => prop.DbInfo.MapTeleportID == this.Scene.EntranceId);
		// try to find the entrance prop entity
		if (entranceProp != null)
		{
			this.Pos = entranceProp.Position;
			this.Rot = entranceProp.Rotation;
		}
		else
		{
			// fallback to default spawn pos
			session.c.Alert($"Entrance prop for entrance ID {this.Scene.EntranceId} not found in scene. Resetting player {this.Uid} position to default spawn point.");
			(this.Pos, this.Rot) = this.Scene.GetDefaultSpawnPosAndRot();
		}
		session.SendPacket(new SceneEntityMoveScNotify()
		{
			EntityId = this.GetLeaderEntityId(),
			Motion = new MotionInfo()
			{
				Pos = this.Pos,
				Rot = this.Rot,
			},
		});
		session.player.SavePersistent();
	}

	public PlayerBasicInfo GetBasicInfo()
	{
		PlayerBasicInfo basicInfo = new()
		{
			Nickname = this.Name,
			Level = this.Level,
			Exp = this.Exp,
			WorldLevel = this.WorldLevel,
			// todo: stamina and coins from inventory
		};
		return basicInfo;
	}

	public void EnterMaze(MapEntryRow entrance, uint groupId, uint configId, out Maze? maze)
	{
		this.Scene = new Scene(session, entrance.PlaneID, entrance.FloorID, entrance.ID);
		PropEntity? entranceProp = this.Scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(prop => prop.DbInfo.ID == configId);
		if (entranceProp == null)
		{
			logger.Fail($"Player {this.Uid} tried to enter maze but entrance prop with config ID {configId} not found.");
			maze = null;
			return;
		}
		this.Pos = entranceProp.Position;
		this.Rot = entranceProp.Rotation;
		if (entranceProp.DbInfo.AnchorID != 0 && entranceProp.DbInfo.AnchorGroupID != 0)
		{
			LevelAnchorInfo? anchor = this.Scene.LevelGroups[entranceProp.DbInfo.AnchorGroupID].AnchorList.FirstOrDefault(a => a.ID == entranceProp.DbInfo.AnchorID);
			if (anchor != null)
			{
				this.Pos = new Protocol.Vector()
				{
					X = (int)(anchor.PosX * 1000),
					Y = (int)(anchor.PosY * 1000),
					Z = (int)(anchor.PosZ * 1000),
				};
				this.Rot = new Protocol.Vector()
				{
					X = 0,
					Y = (int)(anchor.RotY * 1000),
					Z = 0,
				};
			}
			else
			{
				logger.Alert($"Anchor with ID {entranceProp.DbInfo.AnchorID} not found in group {entranceProp.DbInfo.AnchorGroupID} for entrance prop {configId}.");
			}
		}
		maze = this.Scene.ToMazeProto();
		this.Scene.PostEnterScene();
	}

	public void EnterMazeByGroupAnchor(MapEntryRow entrance, uint groupId, uint anchorId, out Maze? maze)
	{
		this.Scene = new Scene(session, entrance.PlaneID, entrance.FloorID, entrance.ID);
		LevelAnchorInfo? anchor = this.Scene.LevelGroups[groupId].AnchorList
			.FirstOrDefault(prop => prop.ID == anchorId);
		if (anchor == null)
		{
			logger.Fail($"Player {this.Uid} tried to enter maze but entrance prop with anchor ID {anchorId} not found.");
			maze = null;
			return;
		}
		if (anchor != null)
		{
			this.Pos = new Protocol.Vector()
			{
				X = (int)(anchor.PosX * 1000),
				Y = (int)(anchor.PosY * 1000),
				Z = (int)(anchor.PosZ * 1000),
			};
			this.Rot = new Protocol.Vector()
			{
				X = 0,
				Y = (int)(anchor.RotY * 1000),
				Z = 0,
			};
		}
		else
		{
			logger.Alert($"Anchor with ID {anchorId} not found in group {groupId} for entrance ID {entrance.ID}.");
		}
		maze = this.Scene.ToMazeProto();
		this.Scene.PostEnterScene();
	}

	public void EnterMaze(MapEntryRow entrance, uint groupId, out Maze? maze)
	{
		uint defaultAnchor = GetDefaultAnchorForEntry(entrance);
		uint defaultGroup = GetDefaultGroupForEntry(entrance);
		this.Scene = new Scene(session, entrance.PlaneID, entrance.FloorID, entrance.ID);
		LevelAnchorInfo? anchor = this.Scene.LevelGroups[defaultGroup].AnchorList.FirstOrDefault(i => i.ID == defaultAnchor);
		if (anchor != null)
		{
			this.Pos = new Protocol.Vector()
			{
				X = (int)(anchor.PosX * 1000),
				Y = (int)(anchor.PosY * 1000),
				Z = (int)(anchor.PosZ * 1000),
			};
			this.Rot = new Protocol.Vector()
			{
				X = 0,
				Y = (int)(anchor.RotY * 1000),
				Z = 0,
			};
		}
		else
		{
			logger.Alert($"Anchor with ID {defaultAnchor} not found in group {groupId} for entrance ID {entrance.ID}.");
		}
		maze = this.Scene.ToMazeProto();
		this.Scene.PostEnterScene();
	}

	public void EquipLightcone(PlayerAvatar avatar, ItemLightcone lightcone)
	{
		if (avatar == null || lightcone == null)
			return;
		avatar.EquipGuid = lightcone.Guid;
		lightcone.BelongAvatarId = avatar.AvatarId;
		this.SavePersistent();
	}

	public void UnequipLightcone(PlayerAvatar avatar, ItemLightcone lightcone)
	{
		if (avatar == null || lightcone == null)
			return;
		if (avatar.EquipGuid != lightcone.Guid)
			return; // lightcone not currently equipped to this avatar
		avatar.EquipGuid = 0;
		lightcone.BelongAvatarId = 0;
		this.SavePersistent();
	}

	public uint GetDefaultGroupForEntry(MapEntryRow entry)
	{
		LevelFloorInfo levelFloorInfo = MainApp.resourceManager.LevelFloorInfos[entry.PlaneID][entry.FloorID];
		return levelFloorInfo.StartGroupID;
	}

	public uint GetDefaultAnchorForEntry(MapEntryRow entry)
	{
		LevelFloorInfo levelFloorInfo = MainApp.resourceManager.LevelFloorInfos[entry.PlaneID][entry.FloorID];
		return levelFloorInfo.StartAnchorID;
	}

	public void OnLogin()
	{
		DailyTaskDataScNotify dailyTaskDataScNotify = new();

		// todo: actually assign based on quest conds and save in database
		// for now just dummy data
		int maxTasks = MainApp.resourceManager.DailyMissionReward.Where(i => i.WorldLevel == this.WorldLevel).Count();
		for (int i = 0; i < maxTasks; i++)
		{
			DailyTask dailyTask = GenerateDailyTask();
			if (dailyTask.MainMissionId != 0)
			{
				this.DailyMissionIds.Add(dailyTask.MainMissionId);
				dailyTaskDataScNotify.DailyTaskLists.Add(dailyTask);
			}
		}
		dailyTaskDataScNotify.FinishedNum = (uint)dailyTaskDataScNotify.DailyTaskLists.Count(t => t.IsFinished);
		dailyTaskDataScNotify.IsTakenExtraReward = true; // for testing, normally would check if player has taken extra reward for the day

		session.SendPacket(dailyTaskDataScNotify);
	}

	private DailyTask GenerateDailyTask()
	{
		DailyTask task = new();

		IEnumerable<DailyMissionRandomDataRow> randomDataRow = MainApp.resourceManager.DailyMissionRandomData
			.Where(row => row.DailyMissionID != 0 && !DailyMissionIds.Contains(row.DailyMissionID));
		// todo: filter by conds as well

		if (!randomDataRow.Any())
		{
			session.c.Fail($"No more daily missions available to assign for player {Uid} at world level {WorldLevel}.");
			return task;
		}

		// pick random
		DailyMissionRandomDataRow selectedRow = randomDataRow.OrderBy(_ => Guid.NewGuid()).First();

		task.MainMissionId = selectedRow.DailyMissionID;
		task.IsFinished = true; // for testing, mark all tasks as finished. normally false

		return task;
	}

	public uint ResolveWorldLevel(uint playerLevel)
	{
		WorldLevelRow? row = MainApp.resourceManager.WorldLevelConfig.OrderByDescending(i => i.Level).FirstOrDefault(i =>
			i.MaxPlayerLevel >= playerLevel
		);
		if (row == null)
		{
			session.c.Alert($"No world level config found for player level {playerLevel}. Defaulting to world level 0.");
			return 0;
		}
		return row.Level;
	}
}
