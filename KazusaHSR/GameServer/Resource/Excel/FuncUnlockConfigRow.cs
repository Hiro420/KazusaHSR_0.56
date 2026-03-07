using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class FuncUnlockConfigRow
{
	public uint UnlockID { get; set; }
	public MALBEKCPJPA[] Conditions { get; set; }
}
