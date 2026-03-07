using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.PlayerInfos;

public class ItemLightcone : PlayerItem
{
	public ItemLightcone(Session session, uint materialId) : base(session, materialId)
	{
	}

	public override void Initialize()
	{
		ItemConfig = MainApp.resourceManager.ItemConfigEquipment.First(i => i.ID == ItemId);
	}

	//public override void AllToRsp(GetBagScRsp rsp)
	//{
	//	Equipment equipment = new Equipment
	//	{
	//		UniqueId = this.Tid,
	//		Tid = this.ItemId,
	//		Level = this.Level,
	//		Exp = this.Exp,
	//		Rank = this.Rank,
	//		BelongAvatarId = this.BelongAvatarId,
	//		IsProtected = this.IsProtected,
	//		Promotion = Promotion
	//	};
	//}
}