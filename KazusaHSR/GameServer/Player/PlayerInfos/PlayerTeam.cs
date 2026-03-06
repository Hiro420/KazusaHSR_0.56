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

	public PlayerTeam(Session session, PlayerAvatar leader) // in case it's only 1 character
	   : this(session, new List<PlayerAvatar> { leader }, leader) { }

	public PlayerTeam(Session session, List<PlayerAvatar> avatars, PlayerAvatar leader)
	{
		this.Avatars = avatars;
		this.Leader = leader;
		this.Session = session;
	}

	public PlayerTeam(Session session)
	{
		this.Avatars = new();
		this.Session = session;
	}

	public void RemoveAvatar(Session session, PlayerAvatar avatar)
	{
		if (this.Avatars.Count == 1)
		{
			session.c.LogError("Cannot remove the last avatar from the team");
			return;
		}
		this.Avatars.Remove(avatar);
		if (this.Leader == avatar)
		{
			this.Leader = this.Avatars[0];
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
		PlayerAvatar oldAvatar = this.Avatars[slot];
		this.Avatars[slot] = newAvatar;
		if (this.Leader == oldAvatar)
		{
			this.Leader = newAvatar;
		}

		session.player?.Scene.DespawnAvatarEntity(oldAvatar);

		session.player?.Scene.SpawnAvatarEntity(newAvatar);

		return Retcode.RetSucc;

		//session.player?.SavePersistent();
	}

	public void AddAvatar(Session session, PlayerAvatar avatar)
	{
		if (this.Avatars.Count == 4)
		{
			session.c.LogError("Cannot add more than 4 characters to the team"); // should never happen
			return;
		}
		this.Avatars.Add(avatar);

		session.player?.Scene.SpawnAvatarEntity(avatar);

		//session.player?.SavePersistent();
	}

	public LineupInfo ToTeamProto()
	{
		LineupInfo info = new LineupInfo()
		{
			LeaderSlot = (uint)this.Avatars.IndexOf(this.Leader!),
			Mp = this.MaxMp,
			Index = (uint)(this.Session.player!.teamList.IndexOf(this) + 1),
			Name = "Default Team",
			PlaneId = Session.player.Scene.PlaneId,
		};
		foreach (PlayerAvatar avatar in this.Avatars)
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
