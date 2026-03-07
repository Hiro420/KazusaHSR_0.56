using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class BookSeriesConfigRow
{
	public uint BookSeriesID { get; set; }
	public TextID BookSeries { get; set; }
	public TextID BookSeriesComments { get; set; }
	public uint BookSeriesNum { get; set; }
	public uint BookSeriesWorld { get; set; }
	public bool IsShowInBookshelf { get; set; }
}
