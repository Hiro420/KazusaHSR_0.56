using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class EliteGroupRow
{
	public uint EliteGroup { get; set; }
	public FixPoint AttackRatio { get; set; }
	public FixPoint DefenceRatio { get; set; }
	public FixPoint HPRatio { get; set; }
	public FixPoint SpeedRatio { get; set; }
	public FixPoint StanceRatio { get; set; }
}
