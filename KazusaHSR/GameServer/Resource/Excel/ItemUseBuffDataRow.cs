using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ItemUseBuffDataRow
{
	public uint UseDataID { get; set; }
	public ItemFoodSubType UseSubType { get; set; }
	public ItemFoodTargetType UseTargetType { get; set; }
	public uint MazeBuffID { get; set; }
	public StageParamEntry[] MazeBuffParam { get; set; }
	public uint SatietyValue { get; set; }
	public uint UseMultipleMax { get; set; }
	public bool IsCheckHP { get; set; }
	public string UseEffect { get; set; }
	public float PreviewHPRecoveryPercent { get; set; }
	public bool IsShowItemDesc { get; set; }
	public bool IsShowUseMultipleSlider { get; set; }
}
