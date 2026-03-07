using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MapEntryRow
{
	public uint ID { get; set; }
	public MapEntryType EntranceType { get; set; }
	public uint IsShowInMapMenu { get; set; }
	public uint EntranceGroupID { get; set; }
	public TextID Name { get; set; }
	public TextID MapEntranceName { get; set; }
	public TextID Desc { get; set; }
	public string ImagePath { get; set; }
	public uint[] MiniMapIconHintList { get; set; }
	public uint ShowReward { get; set; }
	public uint PlaneID { get; set; }
	public uint FloorID { get; set; }
	public uint StartGroupID { get; set; }
	public uint StartAnchorID { get; set; }
	public uint TargetMission { get; set; }
	public uint[] TargetMainMissionList { get; set; }
	public uint[] BeginMainMissionList { get; set; }
	public uint[] FinishMainMissionList { get; set; }
	public uint[] FinishQuestList { get; set; }
	public uint UnlockQuest { get; set; }
}
