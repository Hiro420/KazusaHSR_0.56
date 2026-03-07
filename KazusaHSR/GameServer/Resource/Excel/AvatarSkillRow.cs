using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarSkillRow
{
	public uint SkillID { get; set; }
	public TextID SkillName { get; set; }
	public TextID SkillTag { get; set; }
	public TextID SkillTypeDesc { get; set; }
	public uint Level { get; set; }
	public uint MaxLevel { get; set; }
	public string SkillTriggerKey { get; set; }
	public string SkillIcon { get; set; }
	public string SkillTagIcon { get; set; }
	public uint LevelUpLevelCond { get; set; }
	public uint LevelUpRankCond { get; set; }
	public string[] LevelUpCostList { get; set; }
	public TextID SkillDesc { get; set; }
	public uint[] ExtraEffectIDList { get; set; }
	public FixPoint[] ShowStanceList { get; set; }
	public BHANHBMOKGN[] ShowDamageList { get; set; }
	public FGCBANMGAFH[] ShowHealList { get; set; }
	public int InitCoolDown { get; set; }
	public int CoolDown { get; set; }
	public FixPoint SPAdd { get; set; }
	public FixPoint SPBase { get; set; }
	public FixPoint SPMultipleRatio { get; set; }
	public FixPoint BPNeed { get; set; }
	public FixPoint BPAdd { get; set; }
	public FixPoint DelayRatio { get; set; }
	public FixPoint[] ParamList { get; set; }
	public AttackDamageType StanceDamageType { get; set; }
	public AttackType AttackType { get; set; }
}
