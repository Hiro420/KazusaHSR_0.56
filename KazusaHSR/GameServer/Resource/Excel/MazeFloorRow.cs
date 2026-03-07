using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MazeFloorRow
{
	public uint FloorID { get; set; }
	public string FloorName { get; set; }
	public uint BaseFloorID { get; set; }
	public string FloorBGMGroupName { get; set; }
	public string FloorBGMNormalStateName { get; set; }
	public string FloorBGMBusyStateName { get; set; }
	public BAHGJFNCJKL FloorType { get; set; }
}
