using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class GameplayGuideDataConfigRow
{
	public uint ID { get; set; }
	public TextID Desc { get; set; }
	public uint MappingInfoID { get; set; }
	public uint MapEntranceID { get; set; }
	public uint GotoID { get; set; }
}
