using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MonsterTemplateRow
{
	public uint MonsterTemplateID { get; set; }
	public TextID MonsterName { get; set; }
	public string MonsterBaseType { get; set; }
	public string JsonConfig { get; set; }
	public string IconPath { get; set; }
	public string RoundIconPath { get; set; }
	public string ImagePath { get; set; }
	public string PrefabPath { get; set; }
	public uint ConfigType { get; set; }
	public uint NatureID { get; set; }
	public FixPoint AttackBase { get; set; }
	public FixPoint DefenceBase { get; set; }
	public FixPoint HPBase { get; set; }
	public FixPoint SpeedBase { get; set; }
	public FixPoint StanceBase { get; set; }
	public FixPoint CriticalChanceBase { get; set; }
	public FixPoint CriticalDamageBase { get; set; }
	public FixPoint StatusResistanceBase { get; set; }
	public FixPoint MinimumFatigueRatio { get; set; }
	public string AIPath { get; set; }
	public int StanceCount { get; set; }
	public AttackDamageType StanceType { get; set; }
	public FixPoint InitialDelayRatio { get; set; }
	public BBDNFJAJBNC[] AISkillSequence { get; set; }
}
