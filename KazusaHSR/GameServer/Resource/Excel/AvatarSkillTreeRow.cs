using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarSkillTreeRow
{
	public uint PointID { get; set; }
	public uint Level { get; set; }
	public uint AvatarID { get; set; }
	public uint PointType { get; set; }
	public string Anchor { get; set; }
	public uint MaxLevel { get; set; }
	public bool DefaultUnlock { get; set; }
	public uint[] PrePoint { get; set; }
	public AbilityPropertyValue[] StatusAddList { get; set; }
	public ItemConfig[] MaterialList { get; set; }
	public uint AvatarLevelLimit { get; set; }
	public uint AvatarPromotionLimit { get; set; }
	public uint[] LevelUpSkillID { get; set; }
	public string IconPath { get; set; }
	public string PointName { get; set; }
	public string PointDesc { get; set; }
	public string AbilityName { get; set; }
	public ONBJDDDEHME PointTriggerKey { get; set; }
	public FixPoint[] ParamList { get; set; }
}
