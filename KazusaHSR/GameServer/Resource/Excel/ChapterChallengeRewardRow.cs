using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ChapterChallengeRewardRow
{
	public uint ID { get; set; }
	public uint ChapterId { get; set; }
	public uint RewardID { get; set; }
	public uint CompleteNumberRequire { get; set; }
}
