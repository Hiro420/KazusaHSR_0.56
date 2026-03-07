using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MonsterRow
{
	public uint MonsterID { get; set; }
	public uint MonsterTemplateID { get; set; }
	public TextID MonsterName { get; set; }
	public TextID MonsterIntroduction { get; set; }
	public uint MonsterType { get; set; }
	public uint Level { get; set; }
	public uint HardLevelGroup { get; set; }
	public uint EliteGroup { get; set; }
	public MonsterRank Rank { get; set; }
	public FixPoint AttackModifyRatio { get; set; }
	public FixPoint DefenceModifyRatio { get; set; }
	public FixPoint HPModifyRatio { get; set; }
	public FixPoint SpeedModifyRatio { get; set; }
	public FixPoint StanceModifyRatio { get; set; }
	public FixPoint AttackModifyValue { get; set; }
	public FixPoint DefenceModifyValue { get; set; }
	public FixPoint HPModifyValue { get; set; }
	public FixPoint SpeedModifyValue { get; set; }
	public FixPoint StanceModifyValue { get; set; }
	public uint[] SkillList { get; set; }
	public BALFKJFOECI[] CustomValues { get; set; }
	public NILEJILPLPA[] DynamicValues { get; set; }
	public KJCPIKMEFPE[] DebuffResist { get; set; }
	public int StanceCountDelta { get; set; }
	public string[] CustomValueTags { get; set; }
	public AttackDamageType[] StanceWeakList { get; set; }
	public GCINNHHNFMP[] DamageTypeResistance { get; set; }
	public bool Release { get; set; }
}
