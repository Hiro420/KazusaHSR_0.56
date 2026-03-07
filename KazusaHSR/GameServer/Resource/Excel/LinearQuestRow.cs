using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class LinearQuestRow
{
	public uint LinearID { get; set; }
	public uint[] QuestList { get; set; }
	public uint MinLevel { get; set; }
	public uint MaxLevel { get; set; }
}
