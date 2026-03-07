using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class WaypointRow
{
	public uint ID { get; set; }
	public uint ChapterID { get; set; }
	public TextID Name { get; set; }
	public TextID WaypointNum { get; set; }
	public string PrefabName { get; set; }
	public string PrefabPath { get; set; }
	public uint Type { get; set; }
	public uint TypeValue { get; set; }
	public uint[] UnlockIDList { get; set; }
	public uint IconColorID { get; set; }
	public uint WaypointCameraID { get; set; }
	public uint NextWaypoint { get; set; }
}
