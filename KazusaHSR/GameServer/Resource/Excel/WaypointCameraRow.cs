using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class WaypointCameraRow
{
	public uint WaypointCameraID { get; set; }
	public float[] NormalCamPos { get; set; }
	public float[] CloseUpCamPos { get; set; }
}
