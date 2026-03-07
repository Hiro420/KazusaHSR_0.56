using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MazeBuffRow
{
	public uint ID { get; set; }
	public uint BuffSeries { get; set; }
	public uint BuffRarity { get; set; }
	public uint Lv { get; set; }
	public uint LvMax { get; set; }
	public string ModifierName { get; set; }
	public HMNAIENEPBO InBattleBindingType { get; set; }
	public string InBattleBindingKey { get; set; }
	public StageParamEntry[] ParamList { get; set; }
	public string BuffIcon { get; set; }
	public string BuffName { get; set; }
	public string BuffDesc { get; set; }
	public string BuffDescBattle { get; set; }
	public string BuffEffect { get; set; }
	public BNNBONMACPF MazeBuffType { get; set; }
	public bool IsDisplay { get; set; }
}
