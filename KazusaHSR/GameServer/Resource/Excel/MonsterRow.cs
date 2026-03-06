using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MonsterRow
{
	public uint MonsterId { get; set; }
	public uint MonsterTemplateId { get; set; }
	public TextID MonsterName { get; set; }
	public TextID MonsterIntroduction { get; set; }
	public uint HardLevelGroup { get; set; }
	public uint EliteGroup { get; set; }
	public string Rank { get; set; }
	public FixPoint AttackModifyRatio { get; set; }
	public FixPoint DefenceModifyRatio { get; set; }
	public FixPoint HpModifyRatio { get; set; }
	public FixPoint SpeedModifyRatio { get; set; }
	public FixPoint StanceModifyRatio { get; set; }
	public List<uint> SkillList { get; set; }
	public List<object> CustomValues { get; set; }
	public List<object> DynamicValues { get; set; }
	public List<object> DebuffResist { get; set; }
	public List<string> CustomValueTags { get; set; }
	public List<string> StanceWeakList { get; set; }
	public List<object> DamageTypeResistance { get; set; }
	public FixPoint SpeedModifyValue { get; set; }
	public uint? Level { get; set; }
	public FixPoint StanceModifyValue { get; set; }
}