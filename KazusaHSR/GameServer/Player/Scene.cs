using KazusaHSR.GameServer;
using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using KazusaHSR.Resource;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace KazusaHSR.GameServer;

public class Scene
{
	public uint PlaneId;
	public uint FloorId;
	public uint EntranceId;
	public Session session { get; private set; }
	public EntityManager EntityManager { get; }
	public bool isFinishInit { get; set; } = false;

	private uint _nextEntityIndex = 0;
	private readonly object _entityIdLock = new();
	private MazePlaneRow mazePlaneExcel => MainApp.resourceManager.MazePlaneExcel.Find(m => m.PlaneID == PlaneId)!;
	private LevelFloorInfo levelFloorInfo => MainApp.resourceManager.LevelFloorInfos[PlaneId][FloorId];
	public Dictionary<uint, LevelGroupInfo> LevelGroups => GetDefaultLevelGroups();
	public AbilityManager AbilityManager => session.AbilityManager;

	public Scene(Session session, uint planeId, uint FloorId, uint _entrance = 0, bool spawnEntities = true)
	{
		this.session = session;
		this.EntityManager = new EntityManager(session);
		this.PlaneId = planeId;
		this.FloorId = FloorId;

		if (_entrance != 0)
			this.EntranceId = _entrance;
		else
		{
			LevelFloorInfo levelFloor = this.levelFloorInfo;
			MapEntranceRow? entrance = MainApp.resourceManager.MapEntranceExcel
				.FirstOrDefault(e => e.PlaneId == planeId && e.FloorId == FloorId);
			if (entrance != null)
			{
				this.EntranceId = entrance.Id;
			}
			else
			{
				throw new Exception($"No entrance found for PlaneId {planeId} and FloorId {FloorId}");
			}
		}
		if (spawnEntities)
			this.AddAllEntities();
	}

	public uint GenNewEntityId(EntityType type)
	{
		lock (_entityIdLock)
		{
			_nextEntityIndex++;
			if (_nextEntityIndex > 0xFFFFFF)
			{
				_nextEntityIndex = 1;
			}

			return _nextEntityIndex | ((uint)type << 24);
		}
	}

	public Dictionary<uint, LevelGroupInfo> GetDefaultLevelGroups()
	{
		return GetDefaultLevelGroups(this.PlaneId, this.FloorId);
	}

	public static Dictionary<uint, LevelGroupInfo> GetDefaultLevelGroups(uint planeId, uint floorId)
	{
		LevelFloorInfo levelFloor = MainApp.resourceManager.LevelFloorInfos[planeId][floorId];
		Dictionary<uint, LevelGroupInfo> defaultGroups = new();
		foreach (uint groupId in levelFloor.DefaultGroupIDList.Concat(levelFloor.UnlockMainMissionGroupIDList))
		{
			LevelGroupInfo groupInfo = MainApp.resourceManager.LevelGroups[planeId][floorId][groupId];
			// todo: check for mission conds
			defaultGroups[groupId] = groupInfo;
		}
		//foreach (LevelGroupInstanceInfo info in levelFloor.GroupList)
		//{
		//	LevelGroupInfo groupInfo = MainApp.resourceManager.LevelGroups[planeId][floorId][info.ID];
		//	defaultGroups[info.ID] = groupInfo;
		//}
		return defaultGroups;
	}

	public SceneInfo ToSceneInfoProto()
	{
		SceneInfo info = new SceneInfo()
		{
			PlaneId = this.PlaneId,
			FloorId = this.FloorId,
			LeaderEntityId = this.session.player!.GetLeaderEntityId(),
			// brute-force all sections by setting them to all ints between 0 and 31
			// not ideal, but I was too lazy to figure out how sections actually work and this is good enough (for now)
			LightenSectionLists = Enumerable.Range(0, 32).Select(i => (uint)i).ToArray(),
			MapId = this.mazePlaneExcel.MapID,
			EntryId = this.EntranceId,
		};
		foreach (var entity in this.EntityManager.Entities.Values)
		{
			info.EntityLists.Add(entity.ToSceneEntityInfo());
		}
		return info;
	}

	public void AddAllEntities()
	{
		foreach (PlayerAvatar avatar in this.session.player!.GetCurrentLineup().Avatars.Where(a => a != null))
		{
			this.EntityManager.Add(new AvatarEntity(this.session, avatar));
		}
		foreach (KeyValuePair<uint, LevelGroupInfo> groupInfo in this.LevelGroups)
		{
			SpawnAllProps(groupInfo.Value, groupInfo.Key);
			SpawnAllNPCs(groupInfo.Value, groupInfo.Key);
			SpawnAllMonsters(groupInfo.Value, groupInfo.Key);
		}
	}

	public void SpawnAllProps(LevelGroupInfo groupInfo, uint groupId, bool broadcastPacket = false)
	{
		foreach (LevelPropInfo propInfo in groupInfo.PropList ?? Enumerable.Empty<LevelPropInfo>())
		{
			if (!propInfo.CreateOnInitial)
				continue;
			BaseEntity propEntity = new PropEntity(this.session, propInfo, groupId);
			this.EntityManager.Add(propEntity);
		}
	}

	public void SpawnAllNPCs(LevelGroupInfo groupInfo, uint groupId, bool broadcastPacket = false)
	{
		foreach (LevelNPCInfo npcInfo in groupInfo.NPCList ?? Enumerable.Empty<LevelNPCInfo>())
		{
			if (!npcInfo.CreateOnInitial)
				continue;
			if (!MainApp.resourceManager.NpcExcel.Any(n => n.Id == npcInfo.NPCID))
				continue;
			BaseEntity npcEntity = new NpcEntity(this.session, npcInfo, groupId);
			this.EntityManager.Add(npcEntity);
		}
	}

	public void SpawnAllMonsters(LevelGroupInfo groupInfo, uint groupId, bool broadcastPacket = false)
	{
		foreach (LevelMonsterInfo monsterInfo in groupInfo.MonsterList ?? Enumerable.Empty<LevelMonsterInfo>())
		{
			if (!monsterInfo.CreateOnInitial)
				continue;
			BaseEntity monsterEntity = new MonsterEntity(this.session, monsterInfo, groupId);
			this.EntityManager.Add(monsterEntity);
		}
	}

	public LevelAnchorInfo GetDefaultAnchor()
	{
		LevelFloorInfo levelFloor = this.levelFloorInfo;
		uint startGroupId = levelFloor.StartGroupID;
		uint startAnchorId = levelFloor.StartAnchorID;
		LevelGroupInfo groupInfo = MainApp.resourceManager.LevelGroups[this.PlaneId][this.FloorId][startGroupId];
		LevelAnchorInfo anchorInfo = groupInfo.AnchorList.First(a => a.ID == startAnchorId)!;
		return anchorInfo;
	}

	public (Protocol.Vector, Protocol.Vector) GetDefaultSpawnPosAndRot()
	{
		LevelAnchorInfo anchorInfo = GetDefaultAnchor();
		return (new Protocol.Vector()
		{
			X = (int)(anchorInfo.PosX * 1000),
			Y = (int)(anchorInfo.PosY * 1000),
			Z = (int)(anchorInfo.PosZ * 1000),
		}, new Protocol.Vector()
		{
			X = 0,
			Y = (int)(anchorInfo.RotY * 1000),
			Z = 0,
		});
	}

	public Maze ToMazeProto()
	{
		Maze maze = new Maze()
		{
			Floor = new MazeFloor()
			{
				FloorId = this.FloorId,
				Scene = this.ToSceneInfoProto(),
			},
			MapEntryId = EntranceId,
			// todo: maze id?
		};
		return maze;
	}

	public void SpawnAvatarEntity(PlayerAvatar avatar)
	{
		AvatarEntity avatarEntity = new AvatarEntity(this.session, avatar);
		this.EntityManager.Add(avatarEntity);
		session.SendPacket(new SceneEntityUpdateScNotify()
		{
			EntityLists = { avatarEntity.ToSceneEntityInfo() }
		});
	}

	public void DespawnAvatarEntity(PlayerAvatar avatar)
	{
		AvatarEntity? avatarEntity = this.EntityManager.TryGetByPlayerAvatar(avatar);
		if (avatarEntity != null)
		{
			session.SendPacket(new SceneEntityDisappearScNotify() 
			{ 
				EntityIdLists = [avatarEntity._EntityId] 
			});
			this.EntityManager.Remove(avatarEntity._EntityId);
		}
	}
}