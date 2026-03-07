using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ShopGoodsConfigRow
{
	public uint GoodsID { get; set; }
	public uint ItemID { get; set; }
	public uint ItemCount { get; set; }
	public uint Level { get; set; }
	public uint Rank { get; set; }
	public uint[] CurrencyList { get; set; }
	public uint[] CurrencyCostList { get; set; }
	public uint GoodsSortID { get; set; }
	public LimitType LimitType1 { get; set; }
	public uint[] LimitValue1List { get; set; }
	public LimitType LimitType2 { get; set; }
	public uint[] LimitValue2List { get; set; }
	public uint LimitTimes { get; set; }
	public bool CanBeRefresh { get; set; }
	public uint RefreshType { get; set; }
	public string BeginTime { get; set; }
	public string EndTime { get; set; }
	public uint ShopID { get; set; }
}
