using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class DailyMissionRewardRow
{
	public uint WorldLevel { get; set; }
	public uint FinishCount { get; set; }
	public uint RewardID { get; set; }
	public uint ExtraRewardID { get; set; }
}
