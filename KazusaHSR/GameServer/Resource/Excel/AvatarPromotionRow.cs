using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarPromotionRow
{
	public uint AvatarID { get; set; }
	public uint Promotion { get; set; }
	public ItemConfig[] PromotionCostList { get; set; }
	public uint MaxLevel { get; set; }
	public uint WorldLevelRequire { get; set; }
	public uint PlayerLevelRequire { get; set; }
	public FixPoint AttackBase { get; set; }
	public FixPoint AttackAdd { get; set; }
	public FixPoint DefenceBase { get; set; }
	public FixPoint DefenceAdd { get; set; }
	public FixPoint HPBase { get; set; }
	public FixPoint HPAdd { get; set; }
	public FixPoint SpeedBase { get; set; }
	public FixPoint SpeedAdd { get; set; }
	public FixPoint RankAttackBase { get; set; }
	public FixPoint RankAttackAdd { get; set; }
	public FixPoint RankDefenceBase { get; set; }
	public FixPoint RankDefenceAdd { get; set; }
	public FixPoint RankHPBase { get; set; }
	public FixPoint RankHPAdd { get; set; }
	public FixPoint RankSpeedBase { get; set; }
	public FixPoint RankSpeedAdd { get; set; }
	public FixPoint CriticalChance { get; set; }
	public FixPoint CriticalDamage { get; set; }
	public FixPoint RankCriticalChance { get; set; }
	public FixPoint RankCriticalDamage { get; set; }
	public FixPoint MinimumFatigueRatio { get; set; }
	public FixPoint BaseAggro { get; set; }
}
