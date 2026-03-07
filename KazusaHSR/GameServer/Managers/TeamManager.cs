using System;
using System.Collections.Generic;
using System.Linq;
using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public class TeamManager
{
    private readonly Player _player;

    public TeamManager(Player player)
    {
        _player = player ?? throw new ArgumentNullException(nameof(player));
    }

    public IReadOnlyList<PlayerTeam> Teams => _player.teamList;

    public int TeamCount => _player.teamList.Count;

    public void Clear()
    {
        _player.teamList.Clear();
    }

    public void AddTeam(PlayerTeam team)
    {
        if (team == null) throw new ArgumentNullException(nameof(team));
        _player.teamList.Add(team);
    }

    public void EnsureAtLeastOneTeam()
    {
        if (_player.teamList.Count == 0)
        {
            _player.teamList.Add(new PlayerTeam(_player.Session));
        }
    }

    public int GetIndex(PlayerTeam team)
    {
        if (team == null) throw new ArgumentNullException(nameof(team));
        return _player.teamList.IndexOf(team);
    }

    public PlayerTeam GetTeamByIndex(int index)
    {
        if (index < 0 || index >= _player.teamList.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        return _player.teamList[index];
    }

    public Retcode ChangeLeader(int teamIndex, int slotIndex)
    {
        if (teamIndex < 0 || teamIndex >= _player.teamList.Count)
            return Retcode.RetLineupInvalidIndex;

        var team = _player.teamList[teamIndex];
        if (slotIndex < 0 || slotIndex >= team.Avatars.Count)
            return Retcode.RetLineupAvatarNotExist;

        var avatar = team.Avatars[slotIndex];
        team.SetLeader(_player.Session, avatar);
        _player.SavePersistent();
        return Retcode.RetSucc;
    }

    public Retcode SetAvatarInSlot(int teamIndex, int slotIndex, PlayerAvatar avatar)
    {
        if (avatar == null)
            throw new ArgumentNullException(nameof(avatar));

        if (teamIndex < 0 || teamIndex >= _player.teamList.Count)
            return Retcode.RetLineupInvalidIndex;

        var team = _player.teamList[teamIndex];
        var ret = team.SqueezeAvatarIn(_player.Session, avatar, slotIndex);
        if (ret == Retcode.RetSucc)
            _player.SavePersistent();

        return ret;
    }

    public Retcode RemoveAvatar(int teamIndex, int slotIndex)
    {
        if (teamIndex < 0 || teamIndex >= _player.teamList.Count)
            return Retcode.RetLineupInvalidIndex;

        var team = _player.teamList[teamIndex];
        if (slotIndex < 0 || slotIndex >= team.Avatars.Count)
            return Retcode.RetLineupAvatarNotExist;

        var avatar = team.Avatars[slotIndex];
        if (team.Avatars.Count == 1)
            return Retcode.RetLineupAvatarNotExist;

        team.RemoveAvatar(_player.Session, avatar);
        _player.SavePersistent();
        return Retcode.RetSucc;
    }

    public Retcode SetActiveTeam(uint teamIndex)
    {
        if (teamIndex < 0 || teamIndex >= _player.teamList.Count)
            return Retcode.RetLineupInvalidIndex;

        _player.TeamIndex = teamIndex;
        _player.SavePersistent();
        return Retcode.RetSucc;
    }
}