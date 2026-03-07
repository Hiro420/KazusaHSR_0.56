using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class TrialAdventurePlayerRow
{
	public uint TrialPlayerID { get; set; }
	public uint PlayerID { get; set; }
	public TrialAvatarType Type { get; set; }
	public uint Level { get; set; }
	public uint Promotion { get; set; }
	public uint Rank { get; set; }
	public FOGKHEHLCKN SkillTreeTemplate { get; set; }
	public uint EquipmentID { get; set; }
	public uint EquipmentLevel { get; set; }
	public uint EquipmentPromotion { get; set; }
	public uint EquipmentRank { get; set; }
	public bool IsAutoBattle { get; set; }
	public bool DisableSwitch { get; set; }
	public uint MainMissionID { get; set; }
	public uint PlaneID { get; set; }
	public bool IsProtected { get; set; }
	public string[] AbilityNameList { get; set; }
}
