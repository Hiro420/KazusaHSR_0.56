using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MapEntryGroupRow
{
	public uint ID { get; set; }
	public uint MapGuideID { get; set; }
	public uint Type { get; set; }
	public TextID GroupName { get; set; }
}
