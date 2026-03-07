using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class DamageTypeRow
{
	public AttackDamageType ID { get; set; }
	public TextID DamageTypeName { get; set; }
	public string DamageTypeIconPath { get; set; }
	public string IconNatureForWeakActive { get; set; }
	public string IconNatureForWeakUnactive { get; set; }
	public string HeadButtonSelectBgEff { get; set; }
	public string SPInfoEffBack { get; set; }
	public string SPInfoEffFront { get; set; }
	public string Color { get; set; }
	public string ShaderColor { get; set; }
	public string UnfullColor { get; set; }
	public string QTEBackground { get; set; }
	public string SkillBtnEff { get; set; }
	public string IconNatureColor { get; set; }
	public string SPMazeInfoEffFront { get; set; }
}
