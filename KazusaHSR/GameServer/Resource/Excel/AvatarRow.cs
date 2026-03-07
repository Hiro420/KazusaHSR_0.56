using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarRow
{
	public uint AvatarID { get; set; }
	public TextID AvatarName { get; set; }
	public TextID AvatarFullName { get; set; }
	public uint AdventurePlayerID { get; set; }
	public string AvatarVOTag { get; set; }
	public uint Rarity { get; set; }
	public string JsonPath { get; set; }
	public uint NatureID { get; set; }
	public AttackDamageType DamageType { get; set; }
	public FixPoint SPNeed { get; set; }
	public uint ExpGroup { get; set; }
	public uint MaxPromotion { get; set; }
	public uint MaxRank { get; set; }
	public string[] RankUpCostList { get; set; }
	public uint MaxRankRepay { get; set; }
	public uint[] SkillList { get; set; }
	public AvatarBaseType AvatarBaseType { get; set; }
	public string DefaultAvatarImagePath { get; set; }
	public string DefaultAvatarModelPath { get; set; }
	public string DefaultAvatarHeadIconPath { get; set; }
	public string DefaultAvatarHalfImagePath { get; set; }
	public string AvatarSideIconPath { get; set; }
	public string ActionAvatarHeadIconPath { get; set; }
	public string DefaultAvatarQHeadIconPath { get; set; }
	public string AvatarBaseTypeIconPath { get; set; }
	public string AvatarDialogHalfImagePath { get; set; }
	public string UltraSkillCutInPrefabPath { get; set; }
	public string UIAvatarModelPath { get; set; }
	public string ManikinJsonPath { get; set; }
	public TextID AvatarDesc { get; set; }
	public string AIPath { get; set; }
	public string SkilltreePrefabPath { get; set; }
	public GCINNHHNFMP[] DamageTypeResistance { get; set; }
	public bool Release { get; set; }
	public string SideAvatarHeadIconPath { get; set; }
	public string WaitingAvatarHeadIconPath { get; set; }
	public string AvatarCutinImgPath { get; set; }
	public string AvatarCutinBgImgPath { get; set; }
}
