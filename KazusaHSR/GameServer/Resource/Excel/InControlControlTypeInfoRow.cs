using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class InControlControlTypeInfoRow
{
	public string controlType { get; set; }
	public bool isSettingControlType { get; set; }
	public string iconForSony { get; set; }
	public string iconForXBox { get; set; }
	public string iconForSwitch { get; set; }
}
