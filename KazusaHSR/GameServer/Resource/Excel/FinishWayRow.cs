using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class FinishWayRow
{
	public uint ID { get; set; }
	public uint MazePlaneID { get; set; }
	public uint MazeFloorID { get; set; }
	public GAAOMMCJHGB FinishType { get; set; }
	public APEOHGHKLPA ParamType { get; set; }
	public uint ParamInt1 { get; set; }
	public uint ParamInt2 { get; set; }
	public uint ParamInt3 { get; set; }
	public string ParamStr1 { get; set; }
	public uint[] ParamIntList { get; set; }
	public ItemConfig[] ParamItemList { get; set; }
	public uint Progress { get; set; }
	public bool IsBackTrack { get; set; }
}
