using KazusaHSR.Protocol;

namespace KazusaHSR.GameServer.PlayerInfos;

public class ItemLightcone : PlayerItem
{
	public uint Exp { get; set; }
	public uint Rank { get; set; }
	public uint BelongAvatarId { get; set; }
	public bool IsProtected { get; set; }
	public uint Promotion { get; set; }

	public ItemLightcone(Session session, uint materialId) : base(session, materialId)
	{
		this.Exp = 0;
		this.Rank = 1;
		this.BelongAvatarId = 0;
		this.IsProtected = false;
		this.Promotion = 0;
	}

	public ItemLightcone(Session session, uint materialId, uint guid) : base(session, materialId, guid)
	{
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
}