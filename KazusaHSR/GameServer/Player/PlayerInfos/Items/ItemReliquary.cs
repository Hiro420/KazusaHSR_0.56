using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.PlayerInfos;

public class ItemReliquary : PlayerItem
{
	public ItemReliquary(Session session, uint materialId) : base(session, materialId)
	{
	}

	public override void Initialize()
	{
		ItemConfig = MainApp.resourceManager.ItemConfigEquipment.First(i => i.ID == ItemId);
	}
}
