using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class PlaneEventRow
{
	public uint EventId { get; set; }
	public List<uint> DropList { get; set; }
	public List<uint> DisplayDropItemList { get; set; }
	public uint? WorldLevel { get; set; }
	public uint? Reward { get; set; }
	public uint? StageId { get; set; }
	public uint? MpRecover { get; set; }
	public bool? IsUseMonsterDrop { get; set; }
}