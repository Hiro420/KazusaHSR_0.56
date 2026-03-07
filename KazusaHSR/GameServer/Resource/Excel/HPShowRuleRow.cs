using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class HPShowRuleRow
{
	public uint ID { get; set; }
	public float Max { get; set; }
	public string Color { get; set; }
	public bool IsDanger { get; set; }
}
