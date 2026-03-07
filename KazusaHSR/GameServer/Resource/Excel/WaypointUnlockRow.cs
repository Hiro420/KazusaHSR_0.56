using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class WaypointUnlockRow
{
	public uint ID { get; set; }
	public uint Type { get; set; }
	public string Param { get; set; }
	public uint ParamInt { get; set; }
	public uint[] ParamIntVec { get; set; }
	public TextID UnlockDesc { get; set; }
}
