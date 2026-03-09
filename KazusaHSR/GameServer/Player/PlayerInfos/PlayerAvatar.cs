using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using KazusaHSR.Resource;

namespace KazusaHSR.GameServer.PlayerInfos;

public class PlayerAvatar
{
	private static ResourceManager resourceManager => MainApp.resourceManager;
	public AvatarRow AvatarExcel;
	private Session Session { get; set; }
	public uint Guid { get; set; }
	public uint AvatarId { get; set; }
	public uint Level { get; set; }
	public uint Rank { get; set; }
	public uint Exp { get; set; }
	public uint Hp { get; set; }
	public uint MaxHp { get; set; }
	public uint SP { get; set; }
	public uint EquipGuid { get; set; }
	public uint PromoteLevel { get; set; }
	public uint SkillCastCnt { get; set; }
	public Dictionary<uint, uint> SkilltreeLists { get; set; }

	public PlayerAvatar(Session session, uint AvatarId, uint? tid = null)
	{
		this.Session = session;
		this.Guid = session.GetGuid();
		this.AvatarId = AvatarId;
		this.AvatarExcel = resourceManager.AvatarExcel.Find(a => a.AvatarID == AvatarId)!;
		this.SkilltreeLists = InitSkillTree();
		this.Level = 80;
		this.Exp = 0;
		this.PromoteLevel = 5;
		this.Rank = 6;
		this.MaxHp = GetMaxHp();
		this.Hp = this.MaxHp;
		this.SP = (uint)AvatarExcel.SPNeed.Value;
		this.SkillCastCnt = 0;
	}

	public Protocol.Avatar ToAvatarProto()
	{
		Protocol.Avatar avatar = new Protocol.Avatar
		{
			AvatarId = this.AvatarId,
			Level = this.Level,
			Exp = this.Exp,
			Promotion = this.PromoteLevel,
			EquipmentUniqueId = (uint)this.EquipGuid,
			Rank = this.Rank,
		};
		avatar.SkilltreeLists.AddRange(this.GetSkillLists());
		return avatar;
	}

	public uint GetMaxHp()
	{
		uint baseMaxHp = GetBaseMaxHp();
		foreach (KeyValuePair<uint, uint> item in this.SkilltreeLists)
		{
			if (item.Value == 0)
				continue; // skill tree point not unlocked, skip
			AvatarSkillTreeRow? treeRow = resourceManager.AvatarSkillTreeExcel.Where(s =>
				s.PointID == item.Key &&
				s.Level == item.Value
			).FirstOrDefault();
			if (treeRow != null)
			{
				foreach (AbilityPropertyValue statusAdd in treeRow.StatusAddList)
				{
					switch (statusAdd.PropertyType)
					{
						case AvatarPropertyType.HPAddedRatio:
							baseMaxHp = (uint)(baseMaxHp * (1 + statusAdd.Value.Value));
							break;
							// maybe add other types in the future if needed for other stats
					}
				}
			}
			else
			{
				Session.c.LogWarning(
					$"No AvatarSkillTreeRow config found for PointID {item.Key} with Level {item.Value}");
			}
		}

		// todo: get from relics and lightcone

		return baseMaxHp;
	}

	public uint GetBaseMaxHp()
	{
		AvatarPromotionRow? confg = resourceManager.AvatarPromotionExcel
			.Where(a => a.AvatarID == this.AvatarId && a.Promotion == this.PromoteLevel)
			.OrderByDescending(a => a.MaxLevel)
			.FirstOrDefault();

		if (confg != null)
		{
			uint clampedLevel = this.Level;
			if (clampedLevel < 1)
				clampedLevel = 1;
			if (clampedLevel > confg.MaxLevel)
				clampedLevel = confg.MaxLevel;

			float baseHp = confg.HPBase.Value;
			float addHp = confg.HPAdd.Value;

			float hp = baseHp + addHp * (clampedLevel - 1);

			// Use rounding so we’re close to the in‑game display
			return (uint)Math.Round(hp, MidpointRounding.ToZero);
		}

		Session.c.LogWarning(
			$"No AvatarPromotionRow config found for AvatarId {this.AvatarId} with PromoteLevel {this.PromoteLevel}");
		return 1000; // default value? shouldn't be used since config should always exist, but just in case
	}

	public BattleAvatar ToBattleAvatar()
	{
		BattleAvatar battleAvatar = new BattleAvatar()
		{
			AvatarType = AvatarType.AvatarFormalType,
			Id = this.AvatarId,
			Level = this.Level,
			Rank = this.Rank,
			Index = (uint)this.Session.player.GetCurrentLineup().Avatars.IndexOf(this),
			Hp = this.Hp * 1000,
			Sp = this.SP * 1000,
			Promotion = this.PromoteLevel,
		};
		if (this.EquipGuid != 0 && Session.player.itemDict.TryGetValue(this.EquipGuid, out PlayerItem? item))
		{
			if (item is not ItemLightcone lightcone)
			{
				Session.c.LogError($"Unexpected equip type for item {this.EquipGuid} on avatar {this.AvatarId}. Expected ItemLightcone, got {item.GetType().Name}");
			}
			else
			{
				battleAvatar.EquipmentLists.Add(lightcone.ToBattleEquipment());
			}
		}
		battleAvatar.SkilltreeLists.AddRange(this.GetSkillLists());
		return battleAvatar;
	}

	public List<Protocol.AvatarSkillTree> GetSkillLists()
	{
		List<Protocol.AvatarSkillTree> skillTrees = new List<Protocol.AvatarSkillTree>();
		foreach (var kvp in this.SkilltreeLists)
		{
			if (kvp.Value == 0)
				continue; // skill tree point not unlocked, skip
			skillTrees.Add(new Protocol.AvatarSkillTree
			{
				PointId = kvp.Key,
				Level = kvp.Value
			});
		}
		return skillTrees;
	}

	public Dictionary<uint, uint> InitSkillTree()
	{
		Dictionary<uint, uint> skillTree = new();

		foreach (var group in resourceManager.AvatarSkillTreeExcel
			.Where(s => s.AvatarID == this.AvatarId && s.Level == 1)
			.GroupBy(a => a.PointID))
		{
			var firstRow = group.First();
			skillTree[group.Key] = firstRow.DefaultUnlock ? 1u : 0u;
		}

		return skillTree;
	}
}
