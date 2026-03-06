using KazusaHSR;
using KazusaHSR.GameServer;
using KazusaHSR.GameServer.Resource.Excel;
using KazusaHSR.Protocol;
using KazusaHSR.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	public ulong EquipGuid { get; set; }
	public uint PromoteLevel { get; set; }
	public uint SkillCastCnt { get; set; }

	public PlayerAvatar(Session session, uint AvatarId)
	{
		this.Session = session;
		this.Guid = session.GetGuid();
		this.AvatarId = AvatarId;
		this.AvatarExcel = resourceManager.AvatarExcel.Find(a => a.AvatarId == AvatarId)!;
		this.Level = 80;
		this.Exp = 0;
		this.PromoteLevel = 5;
		this.Rank = 6;
		this.MaxHp = GetMaxHp();
		this.Hp = this.MaxHp;
		this.SP = (uint)AvatarExcel.SpNeed.Value;
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
		return avatar;
	}

	public uint GetMaxHp()
	{
		uint baseMaxHp = GetBaseMaxHp();

		// todo: get from relics and lightcone

		return baseMaxHp;
	}

	public uint GetBaseMaxHp()
	{
		AvatarPromotionRow? confg = resourceManager.AvatarPromotionExcel
			.Where(a => a.AvatarId == this.AvatarId && a.Promotion == this.PromoteLevel)
			.OrderByDescending(a => a.MaxLevel)
			.FirstOrDefault();

		if (confg != null)
		{
			uint clampedLevel = this.Level;
			if (clampedLevel < 1)
				clampedLevel = 1;
			if (clampedLevel > confg.MaxLevel)
				clampedLevel = confg.MaxLevel;

			float baseHp = confg.HpBase.Value;
			float addHp = confg.HpAdd.Value;

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
			// todo: equipment etc
		};
		return battleAvatar;
	}
}
