using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ItemUseDataRow
{
	public uint UseDataID { get; set; }
	public ABGKBHNBCHF UseType { get; set; }
	public uint[] UseParam { get; set; }
}
