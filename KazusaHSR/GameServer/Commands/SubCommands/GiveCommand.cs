using KazusaHSR.GameServer.PlayerInfos;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.ConsoleCommands.Examples;

[ConsoleCommand("give", "Give resources/items to the selected target", "g", RequiresTarget = true)]
public sealed class GiveCommand : IConsoleCommand
{
	public void Execute(CommandExecutionContext context)
	{
		if (context.TargetSession == null || context.TargetUid == null)
		{
			context.Logger.Alert("Give requires a valid target. Use 'target <uid>' first.");
			return;
		}

		if (context.Args.Count == 0)
		{
			context.Logger.Alert("Usage: give <item|avatar|lightcone> [id] [amount] [level]");
			return;
		}

		string action = context.Args[0].ToLowerInvariant();
		string idStr = context.Args.Count > 1 ? context.Args[1] : "";
		string numStr = context.Args.Count > 2 ? context.Args[2] : "1";
		string levelStr = context.Args.Count > 3 ? context.Args[3] : "1";

		if (!uint.TryParse(idStr, out uint id) || !uint.TryParse(numStr, out uint num) || !uint.TryParse(levelStr, out uint level))
		{
			context.Logger.Alert("Invalid parameters. ID, amount, and level must be integers.");
			return;
		}

		switch (action)
		{
			case "item":
				ProcessGiveItem(id, num, level, context);
				break;
			case "avatar":
				ProcessGiveAvatar(id, num, level, context);
				context.TargetSession.player.SendSyncLineupNotify();
				break;
			case "lightcone":
				ProcessGiveLightcone(id, num, level, context);
				break;
		}

		context.TargetSession.player.SavePersistent();
	}

	private void ProcessGiveItem(uint id, uint num, uint level, CommandExecutionContext context)
	{
		ItemRow? itemRow = MainApp.resourceManager.ItemConfig.FirstOrDefault(i => i.ID == id);
		if (itemRow == null || id == 0)
		{
			context.Logger.Alert($"Item with ID {id} not found.");
			return;
		}

		Session? session = context.TargetSession;
		if (session == null)
		{
			context.Logger.Alert("Target session not found.");
			return;
		}

		PlayerItem item;

		if (session.player.itemDict.Values.Any(i => i.ItemId == id))
		{
			// increase the amount instead
			item = session.player.itemDict.Values.First(i => i.ItemId == id);
			item.Count = Math.Min(item.Count + num, itemRow.PileLimit);
			goto DoLogSucc;
		}

		switch (itemRow.ItemType)
		{
			case Resource.Excel.ItemType.Material:
			case Resource.Excel.ItemType.Gift:
			case Resource.Excel.ItemType.Mission:
			case Resource.Excel.ItemType.Book:
			case Resource.Excel.ItemType.Food:
				item = new(session, itemRow.ID);
				item.Count = Math.Min(num, itemRow.PileLimit);
				session.player.itemDict.Add(item.Guid, item);
				goto DoLogSucc;
			case Resource.Excel.ItemType.Virtual:
				// todo: different handling
				item = new(session, itemRow.ID);
				item.Count = Math.Min(num, itemRow.PileLimit);
				session.player.itemDict.Add(item.Guid, item);
				goto DoLogSucc;
			default:
				// skip other item types for now
				return;
		}

	DoLogSucc:
		PlayerSyncScNotify playerSyncScNotify = new PlayerSyncScNotify()
		{
			MaterialLists = { item.ToMaterialProto() }
		};
		MazePlaneEventScNotify notify = new MazePlaneEventScNotify()
		{
			Reward = new RewardData()
			{
				ItemLists =
				{
					new RewardItem()
					{
						ItemId = item.ItemId,
						Level = item.Level,
						Num = num,
						Promotion = 0,
						Rank = 0
					}
				}
			}
		};
		if (session.SendPacket(playerSyncScNotify) && session.SendPacket(notify))
			context.Logger.Emit($"Gave {num} of item ID {id} (level {level}) to target.");
		else
			context.Logger.Alert("Failed to send item sync packet to target.");
	}

	private void ProcessGiveAvatar(uint id, uint num, uint level, CommandExecutionContext context)
	{
		AvatarRow? avatarRow = MainApp.resourceManager.AvatarExcel.FirstOrDefault(i => i.AvatarID == id);
		if (avatarRow == null || id == 0)
		{
			context.Logger.Alert($"Avatar with ID {id} not found.");
			return;
		}

		Session? session = context.TargetSession;
		if (session == null)
		{
			context.Logger.Alert("Target session not found.");
			return;
		}

		if (session.player.avatarDict.Values.Any(a => a.AvatarId == id))
		{
			context.Logger.Alert($"Target already has avatar ID {id}.");
			return;
		}

		PlayerAvatar playerAvatar = new(session, avatarRow.AvatarID);
		AvatarPromotionRow? maxPromo = MainApp.resourceManager.AvatarPromotionExcel.Where(p => p.AvatarID == avatarRow.AvatarID)
			.OrderByDescending(p => p.Promotion)
			.FirstOrDefault();
		uint maxLevel = maxPromo != null ? maxPromo.MaxLevel : 80;
		playerAvatar.Level = Math.Min(maxLevel, level);
		playerAvatar.PromoteLevel = ResolvePromotionAV(avatarRow, level);
		session.player.avatarDict.Add(playerAvatar.Guid, playerAvatar);

		PlayerSyncScNotify playerSyncScNotify = new PlayerSyncScNotify()
		{
			AvatarSync = new AvatarSync()
			{
				AvatarLists = { playerAvatar.ToAvatarProto() }
			}
		};
		AddAvatarScNotify scNotify = new AddAvatarScNotify()
		{
			AvatarId = playerAvatar.AvatarId,
			Rank = playerAvatar.Rank,
		};
		MazePlaneEventScNotify notify = new MazePlaneEventScNotify()
		{
			Reward = new RewardData()
			{
				ItemLists =
				{
					new RewardItem()
					{
						ItemId = playerAvatar.AvatarId,
						Level = playerAvatar.Level,
						Num = 1,
						Promotion = playerAvatar.PromoteLevel,
						Rank = playerAvatar.Rank
					}
				}
			}
		};

		if (session.SendPacket(playerSyncScNotify) && session.SendPacket(scNotify) && session.SendPacket(notify))
			context.Logger.Emit($"Gave avatar ID {id} (level {level}) to target.");
		else
			context.Logger.Alert("Failed to send avatar sync packet to target.");
	}

	private void ProcessGiveLightcone(uint id, uint num, uint level, CommandExecutionContext context)
	{
		ItemRow? itemRow = MainApp.resourceManager.ItemConfigEquipment.FirstOrDefault(i => i.ID == id);
		if (itemRow == null || id == 0)
		{
			context.Logger.Alert($"Item with ID {id} not found.");
			return;
		}

		Session? session = context.TargetSession;
		if (session == null)
		{
			context.Logger.Alert("Target session not found.");
			return;
		}

		List<ItemLightcone> toAdd = [];

		for (int i = 0; i < num; i++)
		{
			ItemLightcone itemLightcone = new(session, itemRow.ID);
			itemLightcone.Level = level;
			itemLightcone.Promotion = ResolvePromotionLC(itemRow, level);
			toAdd.Add(itemLightcone);
			session.player.itemDict.Add(itemLightcone.Guid, itemLightcone);
		}

		PlayerSyncScNotify playerSyncScNotify = new PlayerSyncScNotify()
		{
			EquipmentLists = { }
		};
		foreach (ItemLightcone lc in toAdd)
		{
			playerSyncScNotify.EquipmentLists.Add(lc.ToEquipmentProto());
		}

		MazePlaneEventScNotify notify = new MazePlaneEventScNotify()
		{
			Reward = new RewardData() { ItemLists = { } }
		};

		foreach (var group in toAdd.GroupBy(x => x.ItemId))
		{
			var lgt = group.First();

			notify.Reward.ItemLists.Add(new RewardItem()
			{
				ItemId = lgt.ItemId,
				Level = lgt.Level,
				Num = (uint)group.Count(),
				Promotion = lgt.Promotion,
				Rank = lgt.Rank
			});
		}

		if (session.SendPacket(playerSyncScNotify) && session.SendPacket(notify))
			context.Logger.Emit($"Gave {num} of lightcone ID {id} (level {level}) to target.");
		else
			context.Logger.Alert("Failed to send lightcone sync packet to target.");
	}

	// todo: take in count world level

	private uint ResolvePromotionAV(AvatarRow avatarRow, uint level)
	{
		IEnumerable<AvatarPromotionRow> promotionRows = MainApp.resourceManager.AvatarPromotionExcel.Where(i =>
			i.MaxLevel >= level && i.AvatarID == avatarRow.AvatarID);
		return promotionRows.OrderByDescending(i => i.Promotion).Select(i => i.Promotion).FirstOrDefault();
	}

	private uint ResolvePromotionLC(ItemRow itemRow, uint level)
	{
		IEnumerable<EquipmentPromotionRow> equipmentPromotionRow = MainApp.resourceManager.EquipmentPromotionConfig.Where(i =>
			i.EquipmentID == itemRow.ID && i.MaxLevel >= level);
		return equipmentPromotionRow.OrderByDescending(i => i.Promotion).Select(i => i.Promotion).FirstOrDefault();
	}
}