using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class RecordRefreshRow
{
	public uint RefreshID { get; set; }
	public IFOKCKENNME RefreshType { get; set; }
	public bool IsInteract { get; set; }
	public uint[] RefreshTime { get; set; }
}
