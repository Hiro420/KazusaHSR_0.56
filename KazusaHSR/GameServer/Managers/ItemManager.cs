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

	public PlayerItem AddItem(uint itemId, uint count, uint level = 1)
	{
		if (count == 0)
			throw new ArgumentOutOfRangeException(nameof(count));

		bool isLightcone = MainApp.resourceManager.ItemConfigEquipment.Any(i => i.ID == itemId);

		PlayerItem? existing = _player.itemDict.Values.FirstOrDefault(i => i.ItemId == itemId);
		if (existing != null && !isLightcone)
		{
			existing.Count += count;
			_player.SavePersistent();
			return existing;
		}

		PlayerItem item;
		if (isLightcone)
		{
			item = new ItemLightcone(_player.Session, itemId)
			{
				Count = count,
				Level = level,
			};
		}
		else
		{
			item = new PlayerItem(_player.Session, itemId)
			{
				Count = count,
				Level = level,
			};
		}
		_player.itemDict[item.Guid] = item;
		_player.SavePersistent();
		SendSync(item);
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

	public void SendSync(PlayerItem item)
	{
		PlayerSyncScNotify notify = new PlayerSyncScNotify();
		switch (item)
		{
			case ItemLightcone ligthcone:
				notify.EquipmentLists.Add(ligthcone.ToEquipmentProto());
				break;
			case PlayerItem _:
				notify.MaterialLists.Add(item.ToMaterialProto());
				break;
		}
		_player.Session.SendPacket(notify);
	}
}
