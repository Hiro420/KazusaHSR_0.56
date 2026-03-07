using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class PropStateIcon
{
	public PropState State { get; set; }
	public uint IconID { get; set; }
	public string Color { get; set; }
	public bool ColorInited { get; set; }
	public Color ColorResult { get; set; }
}
