using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class TutorialGuideDataRow
{
	public uint ID { get; set; }
	public string ImagePath { get; set; }
	public TextID DescText { get; set; }
}
