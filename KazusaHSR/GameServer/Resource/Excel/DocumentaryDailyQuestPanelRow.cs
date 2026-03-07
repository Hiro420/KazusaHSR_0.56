using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class DocumentaryDailyQuestPanelRow
{
	public uint DailyID { get; set; }
	public uint[] QuestList { get; set; }
	public TextID PanelDesc { get; set; }
}
