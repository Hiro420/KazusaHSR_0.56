using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using KazusaHSR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace KazusaHSR.GameServer;

public class Player
{
	private Session session { get; set; }
	public Session Session => session;
	private Logger logger = new("Player");
	// Unique peer id for this player within the server process.
	public uint PeerId { get; set; } = 1;
	public string Name { get; set; }
	public uint Level { get; set; }
	public uint WorldLevel { get; set; }
	public uint Uid { get; set; }
	public Dictionary<ulong, PlayerAvatar> avatarDict { get; set; } = new();
	//public Dictionary<ulong, PlayerWeapon> weaponDict { get; set; }
	public Dictionary<ulong, PlayerItem> itemDict { get; set; } = new();
	public Protocol.Vector Pos { get; private set; }
	public Protocol.Vector Rot { get; private set; } // wont actually be used except for scene tp
	public List<PlayerTeam> teamList { get; set; } = new();
	public uint TeamIndex { get; set; } = 0;
	public Scene Scene { get; set; }
	public uint Exp { get; set; }
	public BattleManager battleManager { get; set; }

	public Player(Session session, uint uid)
	{
		this.session = session;
		this.Uid = uid;
		this.Name = "KazusaHSR";
		this.Level = 30;
		this.WorldLevel = 3;
		this.Exp = 0;
		this.Scene = new Scene(session, 10101, 10101001, 0, false); // LevelGroup_P10101_F10101001_G1
		(this.Pos, this.Rot) = this.Scene.GetDefaultSpawnPosAndRot();
		battleManager = new BattleManager(session);
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
			logger.LogError($"Failed to persist player {Uid}: {ex.Message}");
		}
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

	public void GiveAllAvatars()
	{
		foreach (AvatarRow avatarRow in MainApp.resourceManager.AvatarExcel)
		{
			if (avatarRow.Release == false || avatarRow.AvatarId == 1007)
				continue;
			PlayerAvatar playerAvatar = new(session, avatarRow.AvatarId);
			this.avatarDict.Add(playerAvatar.Guid, playerAvatar);
			if (this.GetCurrentLineup().Avatars.Count < 4)
				this.GetCurrentLineup().AddAvatar(session, playerAvatar);
		}
	}

	public void InitTeams()
	{
		for (int i = 0; i < 4; i++) // maybe later change to use config for max teams amount
		{
			this.teamList.Add(new PlayerTeam(session));
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
		return this.teamList[(int)this.TeamIndex];
	}

	public uint GetLeaderEntityId()
	{
		PlayerTeam lineup = GetCurrentLineup();
		AvatarEntity? leaderEntity = FindEntityByPlayerAvatar(lineup.Leader!);
		if (leaderEntity == null)
		{
			logger.LogError("Leader entity not found in scene");
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
			session.c.LogWarning($"Entrance prop for entrance ID {this.Scene.EntranceId} not found in scene. Resetting player {this.Uid} position to default spawn point.");
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

	public void EnterMaze(MapEntranceRow entrance, uint groupId, uint configId, out Maze? maze)
	{
		this.Scene = new Scene(session, entrance.PlaneId, entrance.FloorId, entrance.Id);
		PropEntity? entranceProp = this.Scene.EntityManager.Entities.Values
			.OfType<PropEntity>()
			.FirstOrDefault(prop => prop.DbInfo.ID == configId);
		if (entranceProp == null)
		{
			logger.LogError($"Player {this.Uid} tried to enter maze but entrance prop with config ID {configId} not found.");
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
				logger.LogWarning($"Anchor with ID {entranceProp.DbInfo.AnchorID} not found in group {entranceProp.DbInfo.AnchorGroupID} for entrance prop {configId}.");
			}
		}
		maze = this.Scene.ToMazeProto();
	}
}
