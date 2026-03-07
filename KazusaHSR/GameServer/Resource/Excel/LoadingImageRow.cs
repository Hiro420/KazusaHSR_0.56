using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class LoadingImageRow
{
	public uint ID { get; set; }
	public uint MinLevel { get; set; }
	public uint MaxLevel { get; set; }
	public uint QuestID { get; set; }
	public string ImagePath { get; set; }
}
