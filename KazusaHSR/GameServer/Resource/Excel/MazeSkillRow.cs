using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MazeSkillRow
{
	public uint MazeSkillId { get; set; }
	public string MazeSkillName { get; set; }
	public uint MazeSkilltype { get; set; }
	public string MazeSkillDesc { get; set; }
	public uint RelatedAvatarSkill { get; set; }
	public uint MPCost { get; set; }
	public string SkillTriggerKey { get; set; }
}
