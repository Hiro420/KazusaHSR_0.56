using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarPromotionRow
{
	public uint AvatarId { get; set; }
	public List<object> PromotionCostList { get; set; }
	public uint MaxLevel { get; set; }
	public uint? PlayerLevelRequire { get; set; }
	public FixPoint AttackBase { get; set; }
	public FixPoint AttackAdd { get; set; }
	public FixPoint DefenceBase { get; set; }
	public FixPoint DefenceAdd { get; set; }
	public FixPoint HpBase { get; set; }
	public FixPoint HpAdd { get; set; }
	public FixPoint SpeedBase { get; set; }
	public FixPoint CriticalChance { get; set; }
	public FixPoint CriticalDamage { get; set; }
	public FixPoint BaseAggro { get; set; }
	public uint? Promotion { get; set; }
	public uint? WorldLevelRequire { get; set; }
}
