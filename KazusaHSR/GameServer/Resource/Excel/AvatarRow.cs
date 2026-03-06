using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarRow
{
	public uint AvatarId { get; set; }
	public TextID AvatarName { get; set; }
	public TextID AvatarFullName { get; set; }
	public uint AdventurePlayerId { get; set; }
	public string AvatarVoTag { get; set; }
	public uint Rarity { get; set; }
	public string JsonPath { get; set; }
	public uint NatureId { get; set; }
	public string DamageType { get; set; }
	public FixPoint SpNeed { get; set; }
	public uint ExpGroup { get; set; }
	public uint MaxPromotion { get; set; }
	public uint MaxRank { get; set; }
	public List<string> RankUpCostList { get; set; }
	public uint MaxRankRepay { get; set; }
	public List<uint> SkillList { get; set; }
	public string AvatarBaseType { get; set; }
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
	public string UiAvatarModelPath { get; set; }
	public string ManikinJsonPath { get; set; }
	public TextID AvatarDesc { get; set; }
	public string AiPath { get; set; }
	public string SkilltreePrefabPath { get; set; }
	public List<object> DamageTypeResistance { get; set; }
	public bool Release { get; set; } = false;
	public string SideAvatarHeadIconPath { get; set; }
	public string WaitingAvatarHeadIconPath { get; set; }
	public string AvatarCutinImgPath { get; set; }
	public string AvatarCutinBgImgPath { get; set; }
}
