using System;
using System.Collections.Generic;
using System.Linq;
using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer;

public class ShopManager
{
    private readonly Player _player;
    public Dictionary<uint, ShopInfo> ShopDict { get; set; }
    private static Dictionary<uint, List<ShopGoodsConfigRow>> GoodsByShopID => MainApp.resourceManager.ShopGoodsConfig.GroupBy(
        i => i.ShopID
    ).ToDictionary(i => i.Key, i => i.ToList());

    public ShopManager(Player player)
    {
        _player = player ?? throw new ArgumentNullException(nameof(player));
        ShopDict = new Dictionary<uint, ShopInfo>();
        foreach (uint shopId in GoodsByShopID.Keys)
        {
            ShopDict[shopId] = new ShopInfo(shopId)
            {
                ItemList = GoodsByShopID[shopId].Select(row => new ShopItemInfo(row)).ToList()
            };
        }
    }

    public ShopInfo? GetShopInfo(uint shopId)
    {
        if (ShopDict.TryGetValue(shopId, out var shopInfo))
        {
            return shopInfo;
        }
        return null;
    }

    public Retcode BuyGood(uint shopId, uint goodId, uint count)
    {
        var shopInfo = GetShopInfo(shopId);
        if (shopInfo == null)
            return Retcode.RetShopNotOpen;

        var itemInfo = shopInfo.ItemList.FirstOrDefault(i => i.SourceRow.GoodsID == goodId);
        if (itemInfo == null)
            return Retcode.RetGoodsNotOpen;

        if (count == 0 || count > itemInfo.Count)
            return Retcode.RetItemCountInvalid;

        PlayerItem? playerItem = _player.ItemManager.Items.FirstOrDefault(i => i.ItemId == itemInfo.ItemId);
        if (playerItem != null && playerItem.Count + count > Math.Min(playerItem.ItemConfig.PileLimit, 999))
            return Retcode.RetItemExceedLimit;

		// Check if player has enough currency
		foreach (var price in itemInfo.Price)
        {
            var shopItem = _player.ItemManager.Items.FirstOrDefault(i => i.ItemId == price.Key);
            if (shopItem == null || shopItem.Count < price.Value + count)
                return Retcode.RetItemCostNotEnough;
        }

        // Deduct currency | TODO
        //foreach (var price in itemInfo.Price)
        //{
        //    _player.ItemManager.RemoveItemByGuid(
        //        _player.ItemManager.Items.First(i => i.ItemId == price.Key).Guid,
        //        price.Value * count
        //    );
        //}

		// Add item to player's inventory
        ItemRow? itemRow = MainApp.resourceManager.ItemConfig.FirstOrDefault(i => i.ID == itemInfo.ItemId);
        if (itemRow != null)
		{
            // likely avatar/equipment. we will implement this later
			_player.ItemManager.AddItem(itemInfo.ItemId, itemInfo.Count * count);
		}

        if (itemInfo.BoughtTimes < itemInfo.SourceRow.LimitTimes && itemInfo.SourceRow.LimitTimes != 0)
        {
            itemInfo.BoughtTimes += count;
        }

		return Retcode.RetSucc;
	}

    public List<Protocol.Shop> GetAllShops(uint shopType)
    {
        List<Protocol.Shop> shops = new();
        IEnumerable<uint> shopIds = MainApp.resourceManager.ShopConfig.Where(s => s.ShopType == shopType).Select(s => s.ShopID);
        foreach (uint shopId in shopIds)
        {
			shops.Add(GetShopToProto(shopId));
		}
		return shops;
	}

	public Protocol.Shop GetShopToProto(uint shopId)
    {
        var shopInfo = GetShopInfo(shopId);
        if (shopInfo == null)
            return null!;
        Protocol.Shop shop = new Protocol.Shop()
        {
            ShopId = shopInfo.ShopId,
            BeginTime = 0, // TODO: implement refresh time logic
            EndTime = 1999999999, // TODO: implement refresh time logic
		};
        foreach (ShopItemInfo itemInfo in shopInfo.ItemList)
        {
            if (!itemInfo.isAvailable)
                continue;
            Protocol.Goods goods = new Protocol.Goods()
            {
                GoodsId = itemInfo.SourceRow.GoodsID,
                BuyTimes = itemInfo.BoughtTimes,
                BeginTime = 0, // TODO: implement refresh time logic
                EndTime = 1999999999, // TODO: implement refresh time logic
			};
            shop.GoodsLists.Add(goods);
		}
		return shop;
	}

    public uint GetGoodsBuyTimes(uint shopId, uint goodsId)
    {
        var shopInfo = GetShopInfo(shopId);
        if (shopInfo == null)
            return 0;
        var itemInfo = shopInfo.ItemList.FirstOrDefault(i => i.SourceRow.GoodsID == goodsId);
        if (itemInfo == null)
            return 0;
        return itemInfo.BoughtTimes;
	}
}

public class ShopInfo
{
    public uint ShopId { get; set; }
	// todo: implement refresh time logic, for now make them not refresh at all
	//public uint RefreshTime { get; set; }
    public List<ShopItemInfo> ItemList { get; set; } = new();
    public ShopInfo(uint shopId)
    {
        ShopId = shopId;
        //RefreshTime = refreshTime;
    }
}

public class ShopItemInfo
{
    public uint ItemId { get; set; }
    public uint Count { get; set; }
    public Dictionary<uint, uint> Price { get; set; } = new(); // itemId, count
    public uint BoughtTimes { get; set; } = 0; // for items with purchase limits, not implemented yet
    public ShopGoodsConfigRow SourceRow { get; set; }
	public bool isAvailable => SourceRow.LimitTimes != 0 && BoughtTimes < SourceRow.LimitTimes;
	public ShopItemInfo(ShopGoodsConfigRow row)
    {
        ItemId = row.ItemID;
        Count = row.ItemCount;
        for (int i = 0; i < row.CurrencyList.Length; i++)
        {
            if (i >= row.CurrencyList.Length || i >= row.CurrencyCostList.Length)
                break;
			if (row.CurrencyList[i] != 0 && row.CurrencyCostList[i] != 0)
            {
                Price[row.CurrencyList[i]] = row.CurrencyCostList[i];
			}
		}
        SourceRow = row;
	}
}