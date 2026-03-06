using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class InteractRow
{
	public uint InteractId { get; set; }
	public PropState SrcState { get; set; }
	public List<ItemCost> ItemCostList { get; set; }
	public TextID InteractDesc { get; set; }
	public PropState TargetState { get; set; }
	public bool? IsEvent { get; set; }
}