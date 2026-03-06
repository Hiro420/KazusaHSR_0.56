using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using KazusaHSR.Resource;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace KazusaHSR.GameServer;

public sealed class AbilityManager
{
    private readonly Session _session;

    private static ResourceManager ResourceManager => MainApp.resourceManager;

    // MazeSkillId -> MazeSkillRow, loaded from MazeSkillExcel
    private static readonly IReadOnlyDictionary<uint, MazeSkillRow> MazeSkillById =
        ResourceManager.MazeSkillExcel.ToDictionary(x => x.MazeSkillId);

    // AvatarId -> AdventurePlayerRow, loaded from AdventurePlayerExcel
    private static readonly IReadOnlyDictionary<uint, AdventurePlayerRow> AdventurePlayerByAvatarId =
        ResourceManager.AdventurePlayerExcel.ToDictionary(x => x.AvatarId);

    public AbilityManager(Session session)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    // I love this part so much..
    public void OnAvatarMazeSkill(PlayerAvatar avatar, SceneCastSkillCsReq req, SceneCastSkillScRsp rsp)
    {
        if (avatar == null)
            throw new ArgumentNullException(nameof(avatar));

        // Should never happen since client shouldn't be able to trigger skills for avatars they don't have, but just in case
        if (!AdventurePlayerByAvatarId.TryGetValue(avatar.AvatarId, out var adventurePlayer) ||
            adventurePlayer.MazeSkillIdList == null)
        {
            // No adventure data -> fall back to normal battle skill (wtf?)
            TriggerBattleSkill(avatar, req, rsp);
            return;
        }

        if (req.SkillIndex >= adventurePlayer.MazeSkillIdList.Count)
        {
            // Out-of-range index -> fall back to battle skill (wtf again)
            TriggerBattleSkill(avatar, req, rsp);
            return;
        }

        uint mazeSkillId = adventurePlayer.MazeSkillIdList[(int)req.SkillIndex];

        if (!MazeSkillById.TryGetValue(mazeSkillId, out var mazeSkill))
        {
            // No maze row found -> treat as normal battle skill (wtf again x3)
            TriggerBattleSkill(avatar, req, rsp);
            return;
        }

        switch (mazeSkill.MazeSkilltype)
        {
            case 1:
                // NormalAtk (e.g. LocalPlayer_*_NormalAtkXX)
                // Typically: overworld attack that can trigger combat
                TriggerMazeNormalAttack(avatar, adventurePlayer, mazeSkill, req, rsp);
                break;

            case 2:
                // MazeSkill: pure maze ability (summon prop, add buff, etc.)
                // Execute LocalPlayer maze TaskConfig only, never start fight here.
                TriggerMazeAbility(avatar, adventurePlayer, mazeSkill, req, rsp);
                break;

            default:
                // Shouldn't be any other types, but just in case, treat as normal battle skill (wtf again x4)
                TriggerBattleSkill(avatar, req, rsp);
                break;
        }
    }

    // Fallback for when we can't resolve a proper maze ability to execute.
    // Just trigger battle skill as if the player casted a normal attack or something.
    private void TriggerMazeAbility(PlayerAvatar avatar, AdventurePlayerRow adventurePlayer, MazeSkillRow mazeSkill, SceneCastSkillCsReq req, SceneCastSkillScRsp rsp)
    {
        // Resolve the EntryAbility name from LocalPlayer config based on AvatarId and MazeSkillId
        string entryAbilityName = ResolveEntryAbilityName(adventurePlayer, mazeSkill);

        // We DONT have an EntryAbility to execute :pensive:
		if (string.IsNullOrEmpty(entryAbilityName))
        {
            this._session.c.LogWarning(
                $"No EntryAbility found for AdventurePlayer AvatarId={adventurePlayer.AvatarId} " +
                $"and MazeSkillId={mazeSkill.MazeSkillId}.");
			return;
        }

        // We have an EntryAbility, but no config for it?? Smh
        var abilityConfig = ResourceManager.GetAdventureAbilityConfig(entryAbilityName);
        if (abilityConfig == null)
        {
            // Maybe data issue? Meh, just log and skip executing any ability
            this._session.c.LogWarning(
                $"No AdventureAbilityConfig found for EntryAbility={entryAbilityName} " +
                $"(AdventurePlayer AvatarId={adventurePlayer.AvatarId}, MazeSkillId={mazeSkill.MazeSkillId}).");
            return;
        }

        ExecuteAdventureAbility(avatar, abilityConfig, req, rsp);
    }

    // Similar to TriggerMazeAbility but for normal attacks that can trigger combat
    private void TriggerMazeNormalAttack(PlayerAvatar avatar, AdventurePlayerRow adventurePlayer, MazeSkillRow mazeSkill, SceneCastSkillCsReq req, SceneCastSkillScRsp rsp)
    {
        // Very similar resolution logic as TriggerMazeAbility
        string entryAbilityName = ResolveEntryAbilityName(adventurePlayer, mazeSkill);

        if (string.IsNullOrEmpty(entryAbilityName))
        {
            TriggerBattleSkill(avatar, req, rsp);
            return;
        }

        var abilityConfig = ResourceManager.GetAdventureAbilityConfig(entryAbilityName);
        if (abilityConfig == null)
        {
            TriggerBattleSkill(avatar, req, rsp);
            return;
        }

        ExecuteAdventureAbility(avatar, abilityConfig, req, rsp);
    }

    // LocalPlayer_*_Config.json.SkillList and MazeSkill.SkillTriggerKey
    private static string ResolveEntryAbilityName(AdventurePlayerRow adventurePlayer, MazeSkillRow mazeSkill)
    {
        if (adventurePlayer == null)
            throw new ArgumentNullException(nameof(adventurePlayer));
        if (mazeSkill == null)
            throw new ArgumentNullException(nameof(mazeSkill));

        // WE DO IT PROPERLY!
        var localPlayerConfig = ResourceManager.GetLocalPlayerConfig(adventurePlayer.PlayerJsonPath);
        if (localPlayerConfig?.SkillList == null)
            return string.Empty;

        // Example:
        //   "SkillList": [
        //     { "Name": "NormalAtk", "EntryAbility": "LocalPlayer_Bronya_NormalAtk01" },
        //     { "Name": "MazeSkill", "AdventureSkillType": "MazeSkill", "EntryAbility": "LocalPlayer_Bronya_MazeSkill" },
        //     ...
        //   ]
        //
        // MazeSkill.SkillTriggerKey is "NormalAtk" or "MazeSkill". I couldn't find any other types in the data, so we just match by Name first.
        var skill = localPlayerConfig.SkillList
            .FirstOrDefault(s => string.Equals(s.Name, mazeSkill.SkillTriggerKey, StringComparison.Ordinal));

        // Fallback, shouldn't really happen
        if (skill == null && mazeSkill.MazeSkilltype == 2)
        {
            skill = localPlayerConfig.SkillList
                .FirstOrDefault(s => s.AdventureSkillType == AdventureSkillType.MazeSkill);
        }

        return skill?.EntryAbility ?? string.Empty;
    }

    private void TriggerBattleSkill(PlayerAvatar avatar, SceneCastSkillCsReq req, SceneCastSkillScRsp rsp)
    {
        _session.c.LogWarning($"TriggerBattleSkill | {avatar.AvatarId} {req.SkillIndex}");
		var ctx = new AdventureAbilityContext(_session, avatar, new AdventureAbilityConfig(), req, rsp);
		var executor = new AdventureTaskExecutor(ctx);
        executor.CreateDummyFight();
		return;
	}

    private void ExecuteAdventureAbility(PlayerAvatar avatar, AdventureAbilityConfig abilityConfig, SceneCastSkillCsReq req, SceneCastSkillScRsp rsp)
    {
        var ctx = new AdventureAbilityContext(_session, avatar, abilityConfig, req, rsp);
        var executor = new AdventureTaskExecutor(ctx);

        _session.c.LogInfo($"ExecuteAdventureAbility | {abilityConfig.Name}");
        foreach (var kvp in abilityConfig.Modifiers)
        {
            _session.c.LogInfo($" - Modifier: {kvp.Key} => {kvp.Value.GetType().Name}");
        }

        // TODO: add more tasks
        executor.ExecuteTasks(abilityConfig.OnStart ?? Enumerable.Empty<TaskConfig>());
	}
}