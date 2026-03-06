using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MazePlaneRow
{
	public uint PlaneId { get; set; }
	public string PlaneType { get; set; }
	public uint SubType { get; set; }
	public uint HpPoolType { get; set; }
	public uint MapId { get; set; }
	public string PlaneName { get; set; }
	public uint StartFloorId { get; set; }
	public List<uint> FloorIdList { get; set; }
}