using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MazeFloorConnectivityRow
{
	public uint FromFloorID { get; set; }
	public uint ToFloorID { get; set; }
	public uint WayPointGroupID { get; set; }
	public uint WayPointEntityID { get; set; }
}
