using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarVORow
{
	public string VOTag { get; set; }
	public uint EnterBattle { get; set; }
	public uint TurnBeginNormal { get; set; }
	public uint TurnBeginHealthLow { get; set; }
	public uint TurnBeginSkillReady { get; set; }
	public uint ActionBegin { get; set; }
	public uint ReceiveHealing { get; set; }
	public uint ReceiveBuff { get; set; }
	public uint LinkLead { get; set; }
	public uint LinkLeadSpecial { get; set; }
	public uint LinkSupport { get; set; }
	public uint LinkSupportSpecial { get; set; }
	public uint SkillSelect1 { get; set; }
	public uint SkillSelect2 { get; set; }
	public uint SkillSelectUlt { get; set; }
	public uint ResultLeadKillEnemy { get; set; }
	public uint ResultLeadTriggerEffect { get; set; }
	public uint ResultLeadSkill1 { get; set; }
	public uint ResultLeadSkill2 { get; set; }
	public uint ResultLeadSkillUlt { get; set; }
	public uint ResultLeadEnemyHealthLow { get; set; }
	public uint ResultSupport { get; set; }
	public uint ResultSupportSpecial { get; set; }
	public uint Victory { get; set; }
	public uint Defeat { get; set; }
	public uint Revived { get; set; }
	public uint EncounterHighRisk { get; set; }
	public uint EncounterLowRisk { get; set; }
	public uint UltraReady { get; set; }
	public uint LightHit { get; set; }
}
