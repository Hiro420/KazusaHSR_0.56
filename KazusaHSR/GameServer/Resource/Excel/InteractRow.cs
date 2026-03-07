using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class InteractRow
{
	public uint InteractID { get; set; }
	public PropState SrcState { get; set; }
	public PropState TargetState { get; set; }
	public ItemConfig[] ItemCostList { get; set; }
	public bool IsEvent { get; set; }
	public TextID InteractDesc { get; set; }
}
