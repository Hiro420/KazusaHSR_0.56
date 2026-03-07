using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ItemRarityConfigRow
{
	public ItemRarity Rarity { get; set; }
	public string FrameItemRarityPath { get; set; }
	public string FrameIconRarityPath { get; set; }
	public string FrameItemRarityBgPath { get; set; }
	public string FrameItemRarityColor { get; set; }
	public string LineItemRarityColor { get; set; }
}
