using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ShopGoodsGroupConfigRow
{
	public uint GroupID { get; set; }
	public uint[] GoodsID { get; set; }
	public uint[] Goodsweight { get; set; }
	public bool CanBeRefresh { get; set; }
	public uint RefreshType { get; set; }
	public string BeginTime { get; set; }
	public string EndTime { get; set; }
	public uint ShopID { get; set; }
}
