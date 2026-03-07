using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class StatusRow
{
	public uint StatusID { get; set; }
	public string ModifierName { get; set; }
	public TextID StatusName { get; set; }
	public DFNFPENLIBD StatusType { get; set; }
	public TextID StatusDesc { get; set; }
	public string StatusIconPath { get; set; }
	public TextID StatusEffect { get; set; }
	public bool CanDispel { get; set; }
	public string[] ReadParamList { get; set; }
}
