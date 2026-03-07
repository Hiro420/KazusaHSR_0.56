using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ToastManagerRow
{
	public string FuncName { get; set; }
	public uint Duration { get; set; }
	public uint Priority { get; set; }
	public bool IsinBattle { get; set; }
}
