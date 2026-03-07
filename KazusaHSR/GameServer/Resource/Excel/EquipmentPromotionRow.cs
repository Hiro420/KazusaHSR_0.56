using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class EquipmentPromotionRow
{
	public uint EquipmentID { get; set; }
	public uint Promotion { get; set; }
	public ItemConfig[] PromotionCostList { get; set; }
	public uint WorldLevelRequire { get; set; }
	public uint PlayerLevelRequire { get; set; }
	public uint MaxLevel { get; set; }
	public FixPoint BaseHP { get; set; }
	public FixPoint BaseHPAdd { get; set; }
	public FixPoint BaseAttack { get; set; }
	public FixPoint BaseAttackAdd { get; set; }
	public FixPoint BaseDefence { get; set; }
	public FixPoint BaseDefenceAdd { get; set; }
}
