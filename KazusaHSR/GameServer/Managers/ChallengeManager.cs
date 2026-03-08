using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public class ChallengeManager
{
	private readonly Player _player;
	private readonly Dictionary<uint, PlayerChallenge> _challenges = new();
	private PlayerChallengeVirtualLineup _virtualLineup = new();
	private Maze? _maze = null;
	private Scene? _savedScene = null;
	private Protocol.Vector _savedPos;
	private Protocol.Vector _savedRot;
	private uint _currentChallengeId = 0;
	private Dictionary<ChallengeTargetConfigRow, bool> _challengeTargetConfigs = new();
	private uint _lastRoundCnt = 0;

	public ChallengeManager(Player player)
	{
		_player = player ?? throw new ArgumentNullException(nameof(player));
	}

	public IReadOnlyCollection<PlayerChallenge> Challenges => _challenges.Values;
	public PlayerChallengeVirtualLineup VirtualLineup => GetVirtualLineup();
	public Maze? CurMaze => _maze;
	public bool IsInChallenge => _maze != null && _currentChallengeId != 0;

	public uint GetStars(uint challengeId)
	{
		return _challenges.TryGetValue(challengeId, out var state) ? state.Stars : 0u;
	}

	public void SetStars(uint challengeId, uint stars)
	{
		if (stars > 3)
			stars = 3;

		if (_challenges.TryGetValue(challengeId, out var state))
		{
			if (stars <= state.Stars)
				return;
			state.Stars = stars;
		}
		else
		{
			_challenges[challengeId] = new PlayerChallenge
			{
				ChallengeId = challengeId,
				Stars = stars,
			};
		}

		_player.SavePersistent();
	}

	internal void SetStarsFromPersistence(uint challengeId, uint stars)
	{
		_challenges[challengeId] = new PlayerChallenge
		{
			ChallengeId = challengeId,
			Stars = stars,
		};
	}

	public PlayerChallengeVirtualLineup GetVirtualLineup()
	{
		if (_virtualLineup.AvatarIds == null)
		{
			_virtualLineup.AvatarIds = new List<uint>(new uint[4]);
		}
		else if (_virtualLineup.AvatarIds.Count < 4)
		{
			while (_virtualLineup.AvatarIds.Count < 4)
				_virtualLineup.AvatarIds.Add(0);
		}
		return _virtualLineup;
	}

	public Retcode SetVirtualAvatarInSlot(int slotIndex, PlayerAvatar avatar)
	{
		if (avatar == null)
			throw new ArgumentNullException(nameof(avatar));
		if (slotIndex < 0 || slotIndex >= 4)
			return Retcode.RetLineupInvalidIndex;

		var lineup = GetVirtualLineup();
		lineup.PlaneId = _player.Scene.PlaneId;
		for (int i = 0; i < lineup.AvatarIds.Count; i++)
		{
			if (lineup.AvatarIds[i] == avatar.AvatarId)
				lineup.AvatarIds[i] = 0;
		}
		lineup.AvatarIds[slotIndex] = avatar.AvatarId;
		if (lineup.LeaderAvatarId == 0)
			lineup.LeaderAvatarId = avatar.AvatarId;

		_player.SavePersistent();
		return Retcode.RetSucc;
	}

	public Retcode RemoveVirtualAvatar(uint avatarId)
	{
		var lineup = GetVirtualLineup();
		int index = lineup.AvatarIds.FindIndex(id => id == avatarId);
		if (index < 0)
			return Retcode.RetLineupAvatarNotExist;

		lineup.AvatarIds[index] = 0;
		if (lineup.LeaderAvatarId == avatarId)
			lineup.LeaderAvatarId = lineup.AvatarIds.FirstOrDefault(id => id != 0);

		_player.SavePersistent();
		return Retcode.RetSucc;
	}

	public void Clear()
	{
		_challenges.Clear();
		_virtualLineup = new PlayerChallengeVirtualLineup();
		_maze = null;
		_savedScene = null;
		_currentChallengeId = 0;
	}

	public void LoadFromPersistence(IEnumerable<PlayerChallenge> challenges, PlayerChallengeVirtualLineup? virtualLineup)
	{
		Clear();
		if (challenges != null)
		{
			foreach (var c in challenges)
			{
				SetStarsFromPersistence(c.ChallengeId, c.Stars);
			}
		}
		if (virtualLineup != null)
		{
			_virtualLineup = new PlayerChallengeVirtualLineup
			{
				PlaneId = virtualLineup.PlaneId,
				CurMp = virtualLineup.CurMp,
				MaxMp = virtualLineup.MaxMp,
				LeaderAvatarId = virtualLineup.LeaderAvatarId,
				AvatarIds = new List<uint>(virtualLineup.AvatarIds ?? new List<uint>(new uint[4])),
			};
			if (_virtualLineup.AvatarIds.Count < 4)
			{
				while (_virtualLineup.AvatarIds.Count < 4)
					_virtualLineup.AvatarIds.Add(0);
			}
		}
	}

	public void SendSyncLineupNotify()
	{
		var lineup = new LineupInfo
		{
			//IsVirtual = true,
			//PlaneId = _player.Scene.PlaneId,
			Mp = _virtualLineup.MaxMp,
			Index = 0,
			ExtraLineupType = ExtraLineupType.LineupChallenge,
			//Name = "Challenge",
		};
		var slots = GetVirtualLineup().AvatarIds;
		for (int i = 0; i < slots.Count; i++)
		{
			uint avatarId = slots[i];
			if (avatarId == 0)
				continue;
			var avatar = _player.avatarDict.Values.FirstOrDefault(a => a.AvatarId == avatarId);
			if (avatar == null)
				continue;
			var la = new LineupAvatar
			{
				Slot = (uint)i,
				AvatarType = AvatarType.AvatarFormalType,
				Id = avatar.AvatarId,
				Hp = avatar.Hp * 10,
				Sp = avatar.SP * 1000,
				Satiety = 100,
				SkillCastCnt = avatar.SkillCastCnt,
			};
			lineup.AvatarLists.Add(la);
		}
		if (_virtualLineup.LeaderAvatarId != 0)
		{
			int leaderSlot = slots.FindIndex(id => id == _virtualLineup.LeaderAvatarId);
			if (leaderSlot < 0 && lineup.AvatarLists.Count > 0)
			{
				leaderSlot = (int)lineup.AvatarLists[0].Slot;
			}
			lineup.LeaderSlot = (uint)Math.Max(leaderSlot, 0);
		}
		else if (lineup.AvatarLists.Count > 0)
		{
			lineup.LeaderSlot = lineup.AvatarLists[0].Slot;
		}
		_player.Session.SendPacket(new SyncLineupNotify
		{
			Lineup = lineup,
		});
	}

	public Retcode EnterChallenge(uint challengeId)
	{
		if (IsInChallenge)
			Clear();

		ChallengeMazeConfigRow? row = MainApp.resourceManager.ChallengeMazeExcel.FirstOrDefault(i => i.ID == challengeId);
		if (row == null)
		{
			return Retcode.RetChallengeNotExist;
		}
		MapEntryRow? mapEntrance = MainApp.resourceManager.MapEntranceExcel.FirstOrDefault(i => i.ID == row.MapEntranceID);
		if (mapEntrance == null)
		{
			return Retcode.RetSceneEntryIdNotMatch;
		}

		_savedScene = _player.Scene;
		_savedPos = _player.Pos;
		_savedRot = _player.Rot;
		_currentChallengeId = challengeId;
		InitTargets(row);

		_player.EnterMaze(mapEntrance, _player.GetDefaultGroupForEntry(mapEntrance), out Maze? maze);
		if (maze == null)
		{
			_savedScene = null;
			_currentChallengeId = 0;
			return Retcode.RetServerInternalError;
		}
		this._maze = maze;
		foreach (uint buff in row.MazeBuff)
		{
			_player.Scene.EntityManager.TryGet(_player.GetLeaderEntityId(), out BaseEntity baseEntity);
			if (baseEntity is not AvatarEntity avatarEntity)
			{
				_player.Session.c.LogError($"Entity {baseEntity._EntityId} is not avatar entity. Expected AvatarEntity, got {baseEntity.GetType().Name}");
			}
			else
			{
				avatarEntity.AddMazeBuff(buff, true);
			}
		}
		return Retcode.RetSucc;
	}

	public void InitTargets(ChallengeMazeConfigRow row)
	{
		_challengeTargetConfigs.Clear();
		List<ChallengeTargetConfigRow> challengeTargetConfigRows = row.ChallengeTargetID.Select(c =>
			MainApp.resourceManager.ChallengeTargetConfig.First(x => x.ID == c) // todo: add sanity checks for safety
		).ToList();
		foreach (var trow in challengeTargetConfigRows)
		{
			if (trow.ChallengeTargetType != ChallengeType.ROUNDS)
			{
				_player.Session.c.LogWarning($"Unexpected challenge target with ID {trow.ID}. Expected ROUNDS, got {trow.ChallengeTargetType}");
				continue;
			}
			this._challengeTargetConfigs.Add(trow, false); 
		}
	}

	public void OnBattleResult(uint stageId, BattleStatistics? statistics)
	{
		if (!IsInChallenge || statistics == null)
		{
			return;
		}

		_lastRoundCnt = statistics.RoundCnt;
	}

	public Retcode LeaveChallenge(out Maze? maze)
	{
		maze = null;
		if (!IsInChallenge || _savedScene == null)
		{
			return Retcode.RetChallengeNotExist;
		}

		// Restore previous scene and position/rotation
		_player.Scene = _savedScene;
		_player.SetPosAndRot(_savedPos, _savedRot);

		// Build maze for the restored (overworld) scene
		maze = _player.Scene.ToMazeProto();

		ChallengeSettleNotify notify = new ChallengeSettleNotify()
		{
			ChallengeId = _currentChallengeId,
			IsWin = false,
			Reward = { }, // todo
			Stars = 0, // when losing, you dont gain stars
		};

		_maze = null;
		_savedScene = null;
		_currentChallengeId = 0;

		_player.Session.SendPacket(notify);

		return Retcode.RetSucc;
	}

	public Retcode FinishChallenge(uint challengeId, out ChallengeSettleNotify notify)
	{
		notify = new ChallengeSettleNotify();

		uint rounds = _lastRoundCnt;
		uint starMask = 0;

		int index = 0;
		foreach (var kvp in _challengeTargetConfigs)
		{
			var targetConfig = kvp.Key;

			bool completed = false;

			if (targetConfig.ChallengeTargetType == ChallengeType.ROUNDS)
			{
				if (rounds <= targetConfig.ChallengeTargetParam)
					completed = true;

			}
			else
			{
				completed = false;
			}

			if (completed)
			{
				starMask |= (1u << index);
			}

			index++;
		}

		SetStars(challengeId, starMask);

		notify.ChallengeId = challengeId;
		notify.IsWin = true;
		notify.Stars = starMask;
		notify.Reward = new RewardData();

		return Retcode.RetSucc;
	}
}
