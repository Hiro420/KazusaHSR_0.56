using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MonsterSkillRow
{
	public uint SkillID { get; set; }
	public TextID SkillName { get; set; }
	public string IconPath { get; set; }
	public TextID SkillDesc { get; set; }
	public TextID SkillTypeDesc { get; set; }
	public uint[] ExtraEffectIDList { get; set; }
	public AttackDamageType DamageType { get; set; }
	public string SkillTriggerKey { get; set; }
	public FixPoint SPHitBase { get; set; }
	public FixPoint DelayRatio { get; set; }
	public FixPoint[] ParamList { get; set; }
	public AttackType AttackType { get; set; }
	public uint AI_CD { get; set; }
	public uint AI_ICD { get; set; }
}
