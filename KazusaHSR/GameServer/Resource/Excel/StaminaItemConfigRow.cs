using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class StaminaItemConfigRow
{
	public uint ItemID { get; set; }
	public bool IsAlwaysShown { get; set; }
	public uint SortWeight { get; set; }
	public TextID Desc { get; set; }
}
