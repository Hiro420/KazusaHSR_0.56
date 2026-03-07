using System;
using System.Collections.Generic;
using System.Linq;
using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public class ItemManager
{
    private readonly Player _player;

    public ItemManager(Player player)
    {
        _player = player ?? throw new ArgumentNullException(nameof(player));
    }

    public IReadOnlyCollection<PlayerItem> Items => _player.itemDict.Values;

    public PlayerItem? GetByGuid(uint guid)
    {
        return _player.itemDict.Values.FirstOrDefault(i => i.Guid == guid);
    }

    public PlayerItem AddItem(uint itemId, uint count)
    {
        if (count == 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        PlayerItem? existing = _player.itemDict.Values.FirstOrDefault(i => i.ItemId == itemId);
        if (existing != null)
        {
            existing.Count += count;
            _player.SavePersistent();
            return existing;
        }

        var item = new PlayerItem(_player.Session, itemId, _player.GetNextItemGuid())
        {
            Count = count
        };
        _player.itemDict[item.Guid] = item;
        _player.SavePersistent();
        return item;
    }

    public bool RemoveItemByGuid(uint guid, uint count)
    {
        var item = GetByGuid(guid);
        if (item == null)
            return false;

        if (count >= item.Count)
        {
            _player.itemDict.Remove(item.Guid);
        }
        else
        {
            item.Count -= count;
        }

        _player.SavePersistent();
        return true;
    }

    public void Clear()
    {
        _player.itemDict.Clear();
    }

    public void AddFromPersistence(PlayerItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _player.itemDict[item.Guid] = item;
    }
}
