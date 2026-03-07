using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class StanceLevelEffectRow
{
	public int ID { get; set; }
	public int LevelDifference { get; set; }
	public FixPoint StanceLevelEffect { get; set; }
}
