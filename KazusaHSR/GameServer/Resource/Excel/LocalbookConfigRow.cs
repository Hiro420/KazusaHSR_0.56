using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class LocalbookConfigRow
{
	public uint BookID { get; set; }
	public uint BookSeriesID { get; set; }
	public uint BookSeriesInsideID { get; set; }
	public TextID BookInsideName { get; set; }
	public TextID BookContent { get; set; }
	public uint BookDisplayType { get; set; }
}
