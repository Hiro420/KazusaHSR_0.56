using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class CocoonRow
{
	public uint ID { get; set; }
	public uint WorldLevel { get; set; }
	public uint PropID { get; set; }
	public uint MappingInfoID { get; set; }
	public uint StageID { get; set; }
	public uint[] StageIDList { get; set; }
	public uint[] DropList { get; set; }
	public uint StaminaCost { get; set; }
}
