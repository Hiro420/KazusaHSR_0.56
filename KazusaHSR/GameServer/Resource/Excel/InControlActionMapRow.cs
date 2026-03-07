using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class InControlActionMapRow
{
	public string actionName { get; set; }
	public TextID actionTextmapID { get; set; }
	public uint bindingSettingsType { get; set; }
	public uint actionType { get; set; }
	public bool isSettingComboKey { get; set; }
	public string defaultKey { get; set; }
	public string defaultMouseType { get; set; }
	public string[] defaultInControlTypes { get; set; }
	public uint FuncGotoID { get; set; }
}
