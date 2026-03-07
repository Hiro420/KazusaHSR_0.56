using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class CycleQuestRow
{
	public uint CycleID { get; set; }
	public uint[] QuestList { get; set; }
	public uint MinLevel { get; set; }
	public uint MaxLevel { get; set; }
	public string BeginTime { get; set; }
	public string EndTime { get; set; }
	public uint Cycledays { get; set; }
	public uint FinishedTimes { get; set; }
	public uint[] WeekDayList { get; set; }
}
