using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class UIRedDotRow
{
	public string RedDot { get; set; }
	public string[] RedDotChildren { get; set; }
	public uint Type { get; set; }
	public uint[] Weight { get; set; }
	public uint UnlockID { get; set; }
}
