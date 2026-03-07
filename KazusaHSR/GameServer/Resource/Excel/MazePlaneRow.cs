using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MazePlaneRow
{
	public uint PlaneID { get; set; }
	public PlaneType PlaneType { get; set; }
	public uint SubType { get; set; }
	public uint HPPoolType { get; set; }
	public uint MapID { get; set; }
	public string PlaneName { get; set; }
	public uint StartFloorID { get; set; }
	public uint[] FloorIDList { get; set; }
}
