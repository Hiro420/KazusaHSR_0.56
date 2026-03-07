using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MainMissionTypeConfigRow
{
	public MainMissionType Type { get; set; }
	public TextID TypeName { get; set; }
	public string TypeIcon { get; set; }
	public string TypeColor { get; set; }
}
