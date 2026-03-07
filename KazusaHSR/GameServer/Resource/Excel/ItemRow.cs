using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ItemRow
{
	public uint ID { get; set; }
	public ItemType ItemType { get; set; }
	public ItemRarity Rarity { get; set; }
	public bool isVisible { get; set; }
	public TextID ItemName { get; set; }
	public TextID ItemDesc { get; set; }
	public TextID ItemBGDesc { get; set; }
	public string ItemIconPath { get; set; }
	public string ItemFigureIconPath { get; set; }
	public string ItemCurrencyIconPath { get; set; }
	public bool IsAutoUse { get; set; }
	public uint PileLimit { get; set; }
	public ABGKBHNBCHF UseType { get; set; }
	public uint UseDataID { get; set; }
	public uint SellGroupID { get; set; }
	public bool IsSellable { get; set; }
	public string RecycleTime { get; set; }
}
