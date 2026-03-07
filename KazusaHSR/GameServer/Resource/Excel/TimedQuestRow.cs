using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class TimedQuestRow
{
	public uint TimedID { get; set; }
	public uint[] QuestList { get; set; }
	public uint MinLevel { get; set; }
	public uint MaxLevel { get; set; }
	public string BeginDate { get; set; }
	public string EndDate { get; set; }
	public string BeginDayTime { get; set; }
	public string EndDayTime { get; set; }
	public bool IsNextDay { get; set; }
	public uint[] WeekDayList { get; set; }
}
