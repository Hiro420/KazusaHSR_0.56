using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarSkillRow
{
	public uint SkillId { get; set; }
	public TextID SkillName { get; set; }
	public TextID SkillTag { get; set; }
	public TextID SkillTypeDesc { get; set; }
	public uint Level { get; set; }
	public uint MaxLevel { get; set; }
	public string SkillTriggerKey { get; set; }
	public string SkillIcon { get; set; }
	public string SkillTagIcon { get; set; }
	public List<object> LevelUpCostList { get; set; }
	public TextID SkillDesc { get; set; }
	public List<object> ExtraEffectIdList { get; set; }
	public List<object> ShowStanceList { get; set; }
	public List<object> ShowDamageList { get; set; }
	public List<object> ShowHealList { get; set; }
	public int InitCoolDown { get; set; }
	public int CoolDown { get; set; }
	public FixPoint SpMultipleRatio { get; set; }
	public FixPoint BpNeed { get; set; }
	public FixPoint DelayRatio { get; set; }
	public List<FixPoint> ParamList { get; set; }
	public FixPoint SpBase { get; set; }
	public FixPoint BpAdd { get; set; }
	public string StanceDamageType { get; set; }
	public string AttackType { get; set; }
}