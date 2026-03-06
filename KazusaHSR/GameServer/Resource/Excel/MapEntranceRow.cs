using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MapEntranceRow
{
	public uint Id { get; set; }
	public MapEntryType EntranceType { get; set; }
	public uint? IsShowInMapMenu { get; set; }
	public TextID Name { get; set; }
	public TextID MapEntranceName { get; set; }
	public TextID Desc { get; set; }
	public string ImagePath { get; set; }
	public List<uint> MiniMapIconHintList { get; set; }
	public uint PlaneId { get; set; }
	public uint FloorId { get; set; }
	public List<uint> TargetMainMissionList { get; set; }
	public List<object> BeginMainMissionList { get; set; }
	public List<uint> FinishMainMissionList { get; set; }
	public List<object> FinishQuestList { get; set; }
	public uint? ShowReward { get; set; }
}