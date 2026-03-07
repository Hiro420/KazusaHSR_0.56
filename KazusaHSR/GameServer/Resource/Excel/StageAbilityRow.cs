using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class StageAbilityRow
{
	public uint StageAbilityID { get; set; }
	public string AbilityName { get; set; }
	public StageParamEntry[] ParamList { get; set; }
	public string AbilityIcon { get; set; }
	public TextID AbilityNameText { get; set; }
	public TextID AbilityDescText { get; set; }
}
