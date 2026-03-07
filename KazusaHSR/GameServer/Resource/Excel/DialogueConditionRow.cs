using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class DialogueConditionRow
{
	public uint ID { get; set; }
	public EBOJLDLJBAG Type { get; set; }
	public uint Param1 { get; set; }
	public uint Param2 { get; set; }
}
