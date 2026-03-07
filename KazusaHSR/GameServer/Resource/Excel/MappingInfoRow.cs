using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MappingInfoRow
{
	public uint ID { get; set; }
	public uint WorldLevel { get; set; }
	public MappingInfoType Type { get; set; }
	public TextID Name { get; set; }
	public TextID Desc { get; set; }
	public uint[] ShowMonsterList { get; set; }
	public uint[] RewardList { get; set; }
	public bool IsShowRewardCount { get; set; }
	public bool isShowCleared { get; set; }
}
