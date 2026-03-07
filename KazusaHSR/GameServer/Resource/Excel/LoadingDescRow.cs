using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class LoadingDescRow
{
	public uint ID { get; set; }
	public uint MinLevel { get; set; }
	public uint MaxLevel { get; set; }
	public uint QuestID { get; set; }
	public TextID TitleTextmapID { get; set; }
	public TextID DescTextmapID { get; set; }
}
