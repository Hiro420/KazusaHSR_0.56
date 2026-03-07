using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MapTeleportRow
{
	public uint ID { get; set; }
	public string TeleportName { get; set; }
	public string TeleportDesc { get; set; }
	public string TeleportImagePath { get; set; }
}
