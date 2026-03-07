using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.PlayerInfos;

public class ItemMaterial : PlayerItem
{
	public ItemMaterial(Session session, uint materialId) : base(session, materialId)
	{
	}
}