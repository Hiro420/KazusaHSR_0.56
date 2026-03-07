using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class QuestDataRow
{
	public uint QuestID { get; set; }
	public uint QuestType { get; set; }
	public TextID QuestTitle { get; set; }
	public TextID QuestDisplay { get; set; }
	public string ImagePath { get; set; }
	public uint UnlockProgress { get; set; }
	public NBMHIOMDPMD UnlockType { get; set; }
	public APEOHGHKLPA UnlockParamType { get; set; }
	public uint UnlockParamInt1 { get; set; }
	public uint RewardID { get; set; }
	public uint FinishWayID { get; set; }
	public uint GotoID { get; set; }
}
