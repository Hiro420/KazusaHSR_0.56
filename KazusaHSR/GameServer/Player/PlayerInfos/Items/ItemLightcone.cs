using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.PlayerInfos;

public class ItemLightcone : PlayerItem
{
	public EquipmentRow EquipmentRow => MainApp.resourceManager.EquipmentConfig.FirstOrDefault(row =>
		row.EquipmentID == this.ItemId
	) ?? throw new Exception($"Cannot find EquipmentRow for ItemId {this.ItemId}");

	public uint Exp { get; set; }
	public uint Rank { get; set; }
	public uint BelongAvatarId { get; set; }
	public bool IsProtected { get; set; }
	public uint Promotion { get; set; }

	public ItemLightcone(Session session, uint materialId) : base(session, materialId)
	{
		// maybe would be better checking in EquipmentPromotionConfig? 
		// will do later, cuz it also changes our HP
		this.Exp = 0;
		this.Rank = EquipmentRow.MaxRank;
		this.BelongAvatarId = 0;
		this.IsProtected = false;
		this.Promotion = EquipmentRow.MaxPromotion;

		EquipmentExpTypeRow equipmentExpType = MainApp.resourceManager.EquipmentExpType.Where(row => row.ExpType == EquipmentRow.ExpType)
			.OrderBy(r => r.Level)
			.LastOrDefault()
			?? throw new Exception($"Cannot find EquipmentExpTypeRow for ExpType {EquipmentRow.ExpType}");
		this.Level = equipmentExpType.Level;
	}

	public ItemLightcone(Session session, uint materialId, uint guid) : base(session, materialId, guid)
	{
	}

	// to ensure we levelup correctly, without errors in the middle that would affect the player's inventory
	public class LightconeExpPlan
	{
		public uint TotalExp { get; set; }
		public uint TotalCoinCost { get; set; }

		public List<(uint ItemId, uint Count)> PileItemsToDeduct { get; } = new();
		public List<uint> UniqueItemsToDeduct { get; } = new();
	}

	public override void AllToRsp(GetBagScRsp rsp)
	{
		Equipment equipment = new Equipment
		{
			UniqueId = this.Guid,
			Tid = this.ItemId,
			Level = this.Level,
			Exp = this.Exp,
			Rank = this.Rank,
			BelongAvatarId = this.BelongAvatarId,
			IsProtected = this.IsProtected,
			Promotion = Promotion
		};
		rsp.EquipmentLists.Add(equipment);
	}

	public BattleEquipment ToBattleEquipment()
	{
		return new BattleEquipment
		{
			Id = this.ItemId,
			Level = this.Level,
			Promotion = this.Promotion,
			Rank = this.Rank
		};
	}

	public Protocol.Equipment ToEquipmentProto()
	{
		return new Equipment()
		{
			UniqueId = this.Guid,
			Tid = this.ItemId,
			Level = this.Level,
			Exp = this.Exp,
			Rank = this.Rank,
			BelongAvatarId = this.BelongAvatarId,
			IsProtected = this.IsProtected,
			Promotion = Promotion
		};
	}
	public override void AddToSync(PlayerSyncScNotify ntf)
	{
		ntf.EquipmentLists.Add(ToEquipmentProto());
	}

	public Retcode AddExp(IEnumerable<ItemCost> costDatas)
	{
		var planResult = TryBuildAddExpPlan(costDatas, out var plan);
		if (planResult != Retcode.RetSucc)
		{
			return planResult;
		}
		Retcode applied = ApplyAddExpPlan(plan!);
		if (applied != Retcode.RetSucc)
		{
			Session.c.LogError($"Failed to apply LightconeExpPlan: {applied}");
			return applied;
		}
		return PostAddExp();
	}

	private Retcode TryBuildAddExpPlan(IEnumerable<ItemCost> costDatas, out LightconeExpPlan? plan)
	{
		plan = new LightconeExpPlan();

		foreach (ItemCost costData in costDatas)
		{
			Retcode ret = TryAddCostToPlan(costData, plan);
			if (ret != Retcode.RetSucc)
			{
				plan = null;
				return ret;
			}
		}

		// Optional: validate final exp result before applying
		// Example: check max level / promotion cap / overflow safety
		// uint finalExp = this.Exp + plan.TotalExp;

		// Optional: validate player has enough coins
		// if (Session.player.Coin < plan.TotalCoinCost)
		//     return Retcode.RetItemNotEnough;

		return Retcode.RetSucc;
	}

	private Retcode TryAddCostToPlan(ItemCost costData, LightconeExpPlan plan)
	{
		if (costData.PileItem != null)
		{
			return TryPlanPileItem(costData.PileItem, plan);
		}

		if (costData.UniqueId != 0)
		{
			return TryPlanUniqueItem(costData.UniqueId, plan);
		}

		return Retcode.RetItemNoCost;
	}

	private Retcode TryPlanPileItem(Protocol.PileItem pileItem, LightconeExpPlan plan)
	{
		if (pileItem.ItemId == 0 || pileItem.ItemNum == 0)
		{
			return Retcode.RetItemNoCost;
		}

		EquipmentExpItemRow? row = MainApp.resourceManager.EquipmentExpItemConfig
			.Find(i => i.ItemID == pileItem.ItemId);

		if (row == null)
		{
			Session.c.LogError($"Cannot find EquipmentExpItemRow for ItemID {pileItem.ItemId}");
			return Retcode.RetItemUseConfigNotExist;
		}

		checked
		{
			plan.TotalExp += row.ExpProvide * pileItem.ItemNum;
			plan.TotalCoinCost += row.CoinCost * pileItem.ItemNum;
		}

		plan.PileItemsToDeduct.Add((pileItem.ItemId, pileItem.ItemNum));
		return Retcode.RetSucc;
	}

	private Retcode TryPlanUniqueItem(uint uniqueId, LightconeExpPlan plan)
	{
		if (!Session.player.itemDict.TryGetValue(uniqueId, out PlayerItem? item))
		{
			return Retcode.RetItemNotExist;
		}

		if (item is not ItemLightcone lightcone)
		{
			Session.c.LogError($"Expected ItemLightcone for uniqueId {uniqueId}, but got {item.GetType().Name}");
			return Retcode.RetItemUseConfigNotExist;
		}

		if (lightcone.Guid == this.Guid)
		{
			// should never happen
			return Retcode.RetServerInternalError;
		}

		if (lightcone.IsProtected)
		{
			return Retcode.RetEquipmentLocked;
		}

		checked
		{
			plan.TotalExp += lightcone.EquipmentRow.ExpProvide;
		}

		plan.UniqueItemsToDeduct.Add(uniqueId);
		return Retcode.RetSucc;
	}

	private Retcode ApplyAddExpPlan(LightconeExpPlan plan)
	{
		checked
		{
			this.Exp += plan.TotalExp;
		}

		foreach (var pile in plan.PileItemsToDeduct)
		{
			uint itemId = pile.ItemId;
			uint count = pile.Count;

			// Session.player.RemovePileItem(itemId, count);
		}

		foreach (uint uniqueId in plan.UniqueItemsToDeduct)
		{
			// TODO
			// Session.player.RemoveItem(uniqueId);
		}

		if (plan.TotalCoinCost > 0)
		{
			// TODO
			// Session.player.SubCoin(plan.TotalCoinCost);
		}

		return Retcode.RetSucc;
	}

	public Retcode PostAddExp()
	{
		EquipmentRow row = this.EquipmentRow;
		uint expType = row.ExpType;

		var expRows = MainApp.resourceManager.EquipmentExpType
			.Where(r => r.ExpType == expType)
			.OrderBy(r => r.Level)
			.ToList();

		if (expRows.Count == 0)
		{
			Session.c.LogError($"Cannot find EquipmentExpType rows for ExpType {expType}");
			return Retcode.RetItemUseConfigNotExist;
		}

		uint maxLevel = expRows.Max(r => r.Level);

		if (this.Level == 0 || this.Level > maxLevel)
		{
			Session.c.LogError($"Invalid current level {this.Level} for ExpType {expType}");
			return Retcode.RetItemUseConfigNotExist;
		}

		Dictionary<uint, uint> expCostByLevel = expRows.ToDictionary(r => r.Level, r => r.Exp);

		while (this.Level < maxLevel)
		{
			if (!expCostByLevel.TryGetValue(this.Level, out uint needExp))
			{
				Session.c.LogError($"Missing exp cost for ExpType {expType} at Level {this.Level}");
				return Retcode.RetItemUseConfigNotExist;
			}

			if (this.Exp < needExp)
			{
				break;
			}

			this.Exp -= needExp;
			this.Level++;
		}

		if (this.Level >= maxLevel)
		{
			this.Level = maxLevel;
			this.Exp = 0;
		}

		return Retcode.RetSucc;
	}
}