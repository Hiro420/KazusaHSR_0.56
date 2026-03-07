using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ShopConfigRow
{
	public uint ShopID { get; set; }
	public uint ShopType { get; set; }
	public TextID ShopName { get; set; }
	public TextID ShopDesc { get; set; }
	public string ShopIconPath { get; set; }
	public string ShopBar { get; set; }
	public uint ShopSortID { get; set; }
	public LimitType LimitType1 { get; set; }
	public uint[] LimitValue1List { get; set; }
	public LimitType LimitType2 { get; set; }
	public uint[] LimitValue2List { get; set; }
	public string BeginTime { get; set; }
	public string EndTime { get; set; }
	public bool IsOpen { get; set; }
	public bool ServerVerification { get; set; }
}
