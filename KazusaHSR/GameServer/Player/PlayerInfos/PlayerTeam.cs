using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.PlayerInfos;

public class PlayerTeam
{
	public Session Session { get; private set; }
	public PlayerAvatar? Leader { get; private set; }
	public List<PlayerAvatar> Avatars { get; private set; }
	public uint CurMp { get; set; } = 5;
	public uint MaxMp { get; set; } = 5;

	public PlayerTeam(Session session)
	{
		this.Session = session;
		this.Avatars = new List<PlayerAvatar>(4) { null, null, null, null };
	}

	public PlayerTeam(Session session, PlayerAvatar leader)
		: this(session)
	{
		this.Leader = leader;
		this.Avatars[0] = leader;
	}

	public PlayerTeam(Session session, List<PlayerAvatar> avatars, PlayerAvatar leader)
		: this(session)
	{
		for (int i = 0; i < avatars.Count && i < 4; i++)
		{
			this.Avatars[i] = avatars[i];
		}
		this.Leader = leader;
	}

	public void RemoveAvatar(Session session, PlayerAvatar avatar)
	{
		int activeCount = this.Avatars.Count(a => a != null);
		if (activeCount <= 1)
		{
			session.c.LogError("Cannot remove the last avatar from the team");
			return;
		}
		int index = this.Avatars.IndexOf(avatar);
		if (index < 0)
		{
			return;
		}
		this.Avatars[index] = null;
		for (int i = index; i < this.Avatars.Count - 1; i++)
		{
			this.Avatars[i] = this.Avatars[i + 1];
		}
		this.Avatars[this.Avatars.Count - 1] = null;

		if (this.Leader?.AvatarId == avatar.AvatarId)
		{
			this.Leader = this.Avatars.FirstOrDefault(a => a != null);
		}

		session.player?.Scene.DespawnAvatarEntity(avatar);

		//session.player?.SavePersistent();
	}

	public void SetLeader(Session session, PlayerAvatar avatar)
	{
		if (!this.Avatars.Contains(avatar))
		{
			session.c.LogError("Cannot set leader to an avatar that is not in the team");
			return;
		}
		this.Leader = avatar;
		
		//session.player?.SavePersistent();
	}

	public Retcode SqueezeAvatarIn(Session session, PlayerAvatar newAvatar, int slot)
	{
		if (slot < 0 || slot >= 4)
		{
			session.c.LogError("Slot must be between 0 and 3");
			return Retcode.RetLineupInvalidIndex;
		}
		PlayerAvatar oldAvatar = this.Avatars.Count > slot ? this.Avatars[slot] : null;
		if (this.Avatars.Count <= slot)
		{
			while (this.Avatars.Count <= slot && this.Avatars.Count < 4)
			{
				this.Avatars.Add(null);
			}
		}
		this.Avatars[slot] = newAvatar;
		if (this.Leader == oldAvatar || this.Leader == null)
		{
			this.Leader = newAvatar;
		}

		if (oldAvatar != null)
		{
			session.player?.Scene.DespawnAvatarEntity(oldAvatar);
		}

		session.player?.Scene.SpawnAvatarEntity(newAvatar);

		return Retcode.RetSucc;

		//session.player?.SavePersistent();
	}

	public void AddAvatar(Session session, PlayerAvatar avatar)
	{
		if (this.Avatars.Count(a => a != null) == 4)
		{
			session.c.LogError("Cannot add more than 4 characters to the team"); // should never happen
			return;
		}
		int index = this.Avatars.FindIndex(a => a == null);
		if (index >= 0)
		{
			this.Avatars[index] = avatar;
		}
		else if (this.Avatars.Count < 4)
		{
			this.Avatars.Add(avatar);
		}

		session.player?.Scene.SpawnAvatarEntity(avatar);

		//session.player?.SavePersistent();
	}

	public LineupInfo ToTeamProto()
	{
		LineupInfo info = new LineupInfo()
		{
			LeaderSlot = this.Leader != null ? (uint)this.Avatars.IndexOf(this.Leader) : 0,
			Mp = this.MaxMp,
			Index = (uint)this.Session.player!.TeamManager.GetIndex(this),
			Name = "Default Team",
			PlaneId = Session.player.Scene.PlaneId,
		};
		foreach (PlayerAvatar avatar in this.Avatars.Where(a => a != null))
		{
			LineupAvatar lineupAvatar = new LineupAvatar()
			{
				AvatarType = AvatarType.AvatarFormalType,
				Hp = avatar.Hp * 10, // some weird decimal thing going on here
				Id = avatar.AvatarId,
				Satiety = 100,
				Slot = (uint)this.Avatars.IndexOf(avatar),
				Sp = avatar.SP * 1000,
				SkillCastCnt = avatar.SkillCastCnt
			};
			info.AvatarLists.Add(lineupAvatar);
		}

		return info;
	}
}
