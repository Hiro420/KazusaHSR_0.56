using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AdventureMapRow
{
	public uint ID { get; set; }
	public uint MapType { get; set; }
	public string MapName { get; set; }
	public string SceneName { get; set; }
	public string LevelGraphPath { get; set; }
	public string StartLevelArea { get; set; }
	public string PlayerStartAnchorName { get; set; }
}
