using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;
using KazusaHSR.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer;

// I HATE working with databases, but we need to store player data somewhere, so here we are.
public class DatabaseManager
{
    private readonly Logger _logger = new("DatabaseManager");
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<AccountDocument> _accounts;

    public DatabaseManager(AccountDataBaseInfo config)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));

        if (!config.UseInternal)
        {
            _logger.LogWarning("AccountDataBase.UseInternal is false; DatabaseManager will be inactive.");
        }

        _client = new MongoClient(config.Uri);
        _database = _client.GetDatabase(config.Collection);
        _accounts = _database.GetCollection<AccountDocument>("accounts");
    }

    public async Task<AccountDocument?> GetByTokenAsync(string token)
    {
        var filter = Builders<AccountDocument>.Filter.Eq(a => a.Token, token);
        return await _accounts.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<AccountDocument?> GetByAccountIdAsync(string accountId)
    {
        var filter = Builders<AccountDocument>.Filter.Eq(a => a.AccountId, accountId);
        return await _accounts.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<AccountDocument?> GetByUidAsync(uint uid)
    {
        var filter = Builders<AccountDocument>.Filter.Eq(a => a.Uid, uid);
        return await _accounts.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task SavePlayerAsync(string accountId, string token, Player player)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));

        var doc = AccountDocument.FromPlayer(accountId, token, player);

        var existing = await _accounts
            .Find(a => a.Uid == doc.Uid)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (existing != null)
        {
            doc.Id = existing.Id;
            await _accounts.ReplaceOneAsync(
                a => a.Id == doc.Id,
                doc,
                new ReplaceOptions { IsUpsert = true }).ConfigureAwait(false);
        }
        else
        {
            await _accounts.InsertOneAsync(doc).ConfigureAwait(false);
        }
    }

    public async Task<Player> LoadOrCreatePlayerAsync(Session session, string accountId, string token, uint uid)
    {
        if (session == null) throw new ArgumentNullException(nameof(session));

        var filter = Builders<AccountDocument>.Filter.Eq(a => a.Uid, uid);
        var doc = await _accounts.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);

        if (doc == null || doc.Avatars == null || doc.Avatars.Count == 0)
        {
            _logger.LogInfo($"No existing player data for uid {uid}, creating new.");

            var player = new Player(session, uid);
            player.InitTeams();
            player.AddBasicAvatar();
            player.GiveAllAvatars();

			await SavePlayerAsync(accountId, token, player).ConfigureAwait(false);
            return player;
        }

        doc.AccountId = accountId ?? string.Empty;
        doc.Token = token ?? string.Empty;
        await _accounts.ReplaceOneAsync(a => a.Id == doc.Id, doc).ConfigureAwait(false);

        return doc.ToPlayer(session);
    }
    public async Task<AccountDocument> GetOrCreateAccountAsync(string accountId)
    {
        if (string.IsNullOrWhiteSpace(accountId))
            throw new ArgumentException("accountId must not be empty", nameof(accountId));

        var existing = await GetByAccountIdAsync(accountId).ConfigureAwait(false);
        if (existing != null)
        {
            return existing;
        }

        // People start new accounts at level 1 by default
        // I am special however. I like having fun.
        // Let it be 69420. Nice.

        var last = await _accounts
            .Find(FilterDefinition<AccountDocument>.Empty)
            .SortByDescending(a => a.Uid)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        uint newUid = last != null && last.Uid != 0 ? last.Uid + 1 : 69420u;

        var doc = new AccountDocument
        {
            AccountId = accountId,
            Token = string.Empty,
            Uid = newUid,
            Name = accountId,
        };

        await _accounts.InsertOneAsync(doc).ConfigureAwait(false);
        _logger.LogInfo($"Created new account '{accountId}' with uid {newUid}.");
        return doc;
    }

    public async Task UpdateAccountAsync(AccountDocument account)
    {
        if (account == null) throw new ArgumentNullException(nameof(account));

        await _accounts.ReplaceOneAsync(a => a.Id == account.Id, account, new ReplaceOptions { IsUpsert = true })
            .ConfigureAwait(false);
    }
}

[BsonIgnoreExtraElements]
public class AccountDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string AccountId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public uint Uid { get; set; }

    public string Name { get; set; } = string.Empty;
    public uint Level { get; set; }
    public uint WorldLevel { get; set; }
    public uint Exp { get; set; }

    public uint TeamIndex { get; set; }

    public uint LastItemGuid { get; set; }

    public SceneData Scene { get; set; } = new();
    public VectorData Position { get; set; } = new();
    public VectorData Rotation { get; set; } = new();

    public List<PlayerAvatarData> Avatars { get; set; } = new();
    public List<PlayerItemData> Items { get; set; } = new();
    public List<PlayerTeamData> Teams { get; set; } = new();

    public static AccountDocument FromPlayer(string accountId, string token, Player player)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));

        var doc = new AccountDocument
        {
            AccountId = accountId ?? string.Empty,
            Token = token ?? string.Empty,
            Uid = player.Uid,
            Name = player.Name,
            Level = player.Level,
            WorldLevel = player.WorldLevel,
            Exp = player.Exp,
            TeamIndex = player.TeamIndex,
            LastItemGuid = player.LastItemGuid,
            Scene = new SceneData
            {
                PlaneId = player.Scene.PlaneId,
                FloorId = player.Scene.FloorId,
                EntranceId = player.Scene.EntranceId
            },
            Position = VectorData.FromProto(player.Pos),
            Rotation = VectorData.FromProto(player.Rot),
            Avatars = player.avatarDict.Values.Select(PlayerAvatarData.FromAvatar).ToList(),
            Items = player.ItemManager.Items.Select(PlayerItemData.FromItem).ToList(),
            Teams = player.TeamManager.Teams.Select(PlayerTeamData.FromTeam).ToList(),
        };

        return doc;
    }

    public Player ToPlayer(Session session)
    {
        if (session == null) throw new ArgumentNullException(nameof(session));

        var player = new Player(session, Uid)
        {
            Name = this.Name,
            Level = this.Level,
            WorldLevel = this.WorldLevel,
            Exp = this.Exp,
            LastItemGuid = this.LastItemGuid,
        };

        player.avatarDict.Clear();
        player.ItemManager.Clear();
        player.TeamManager.Clear();

        var avatarsById = new Dictionary<uint, PlayerAvatar>();
        foreach (var avatarData in Avatars)
        {
            var avatar = avatarData.ToAvatar(session);
            avatarsById[avatar.AvatarId] = avatar;
            player.avatarDict[avatar.Guid] = avatar;
        }

        foreach (var itemData in Items)
        {
            var item = itemData.ToItem(session);
            player.ItemManager.AddFromPersistence(item);
        }
        foreach (var teamData in Teams)
        {
            var team = new PlayerTeam(session);

            foreach (var avatarId in teamData.AvatarIds)
            {
                if (avatarsById.TryGetValue(avatarId, out var avatar))
                {
                    team.AddAvatar(session, avatar);
                }
            }

            if (team.Avatars.Any(a => a != null) && avatarsById.TryGetValue(teamData.LeaderAvatarId, out var leader))
            {
                team.SetLeader(session, leader);
            }

            team.CurMp = teamData.CurMp;
            team.MaxMp = teamData.MaxMp;

            player.TeamManager.AddTeam(team);
        }

        player.TeamManager.EnsureAtLeastOneTeam();

        if (TeamIndex >= player.TeamManager.TeamCount)
        {
            player.TeamIndex = 0;
        }
        else
        {
            player.TeamIndex = TeamIndex;
        }

        player.Scene = new Scene(session, Scene.PlaneId, Scene.FloorId, Scene.EntranceId, spawnEntities: false);
        player.SetPos(Position.ToProto());
        player.SetRot(Rotation.ToProto());

        return player;
    }
}

public class SceneData
{
    public uint PlaneId { get; set; }
    public uint FloorId { get; set; }
    public uint EntranceId { get; set; }
}

public class VectorData
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public static VectorData FromProto(Vector proto)
    {
        return new VectorData
        {
            X = proto?.X ?? 0,
            Y = proto?.Y ?? 0,
            Z = proto?.Z ?? 0,
        };
    }

    public Vector ToProto()
    {
        return new Vector
        {
            X = this.X,
            Y = this.Y,
            Z = this.Z,
        };
    }
}

public class PlayerAvatarData
{
    public uint AvatarId { get; set; }
    public uint Level { get; set; }
    public uint Rank { get; set; }
    public uint Exp { get; set; }
    public uint Hp { get; set; }
    public uint MaxHp { get; set; }
    public uint Sp { get; set; }
    public uint PromoteLevel { get; set; }
    public uint SkillCastCnt { get; set; }

    public static PlayerAvatarData FromAvatar(PlayerAvatar avatar)
    {
        return new PlayerAvatarData
        {
            AvatarId = avatar.AvatarId,
            Level = avatar.Level,
            Rank = avatar.Rank,
            Exp = avatar.Exp,
            Hp = avatar.Hp,
            MaxHp = avatar.MaxHp,
            Sp = avatar.SP,
            PromoteLevel = avatar.PromoteLevel,
            SkillCastCnt = avatar.SkillCastCnt,
        };
    }

    public PlayerAvatar ToAvatar(Session session)
    {
		var avatar = new PlayerAvatar(session, AvatarId)
        {
            Level = this.Level,
            Rank = this.Rank,
            Exp = this.Exp,
            Hp = this.Hp,
            MaxHp = this.MaxHp,
            SP = this.Sp,
            PromoteLevel = this.PromoteLevel,
            SkillCastCnt = this.SkillCastCnt,
        };

        return avatar;
    }
}

public class PlayerItemData
{
    public uint Guid { get; set; }
    public uint ItemId { get; set; }
    public uint Count { get; set; }

    public static PlayerItemData FromItem(PlayerItem item)
    {
        return new PlayerItemData
        {
			Guid = item.Guid,
            ItemId = item.ItemId,
            Count = item.Count,
        };
    }

    public PlayerItem ToItem(Session session)
    {
		var item = new PlayerItem(session, ItemId, Guid)
        {
            Count = this.Count,
        };

        return item;
    }
}

public class PlayerTeamData
{
    public uint CurMp { get; set; }
    public uint MaxMp { get; set; }
    public uint LeaderAvatarId { get; set; }

    public List<uint> AvatarIds { get; set; } = new();

    public static PlayerTeamData FromTeam(PlayerTeam team)
    {
        return new PlayerTeamData
        {
            CurMp = team.CurMp,
            MaxMp = team.MaxMp,
            LeaderAvatarId = team.Leader?.AvatarId ?? 0,
			AvatarIds = team.Avatars.Where(a => a != null).Select(a => a.AvatarId).ToList(),
        };
    }
}
