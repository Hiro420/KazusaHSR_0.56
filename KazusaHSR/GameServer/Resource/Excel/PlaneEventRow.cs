using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class PlaneEventRow
{
	public uint EventID { get; set; }
	public uint WorldLevel { get; set; }
	public uint StageID { get; set; }
	public uint[] DropList { get; set; }
	public uint Reward { get; set; }
	public bool IsUseMonsterDrop { get; set; }
	public uint AvatarExpReward { get; set; }
	public uint[] DisplayDropItemList { get; set; }
	public uint MPRecover { get; set; }
}
