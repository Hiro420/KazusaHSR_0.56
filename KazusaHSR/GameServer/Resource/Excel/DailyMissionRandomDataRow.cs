using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class DailyMissionRandomDataRow
{
	public uint DailyMissionID { get; set; }
	public TextID PlaneName { get; set; }
	public string ImagePath { get; set; }
	public uint GotoID { get; set; }
}
