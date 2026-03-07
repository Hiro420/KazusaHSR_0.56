using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class EquipmentRow
{
	public uint EquipmentID { get; set; }
	public bool Release { get; set; }
	public TextID EquipmentName { get; set; }
	public TextID EquipmentDesc { get; set; }
	public uint Rarity { get; set; }
	public AvatarBaseType AvatarBaseType { get; set; }
	public uint MaxPromotion { get; set; }
	public uint MaxRank { get; set; }
	public uint ExpType { get; set; }
	public uint SkillID { get; set; }
	public uint ExpProvide { get; set; }
	public uint CoinCost { get; set; }
	public uint RankUpCost { get; set; }
	public string ThumbnailPath { get; set; }
	public string ImagePath { get; set; }
	public float[] ItemRightPanelOffset { get; set; }
	public float[] AvatarDetailOffset { get; set; }
	public float[] BattleDialogOffset { get; set; }
}
