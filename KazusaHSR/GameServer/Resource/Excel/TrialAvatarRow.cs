using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class TrialAvatarRow
{
	public uint TrialID { get; set; }
	public uint AvatarID { get; set; }
	public uint Level { get; set; }
	public uint Rank { get; set; }
	public uint TrialTag { get; set; }
	public uint[] SkillList { get; set; }
	public uint[] SkillTreeList { get; set; }
}
