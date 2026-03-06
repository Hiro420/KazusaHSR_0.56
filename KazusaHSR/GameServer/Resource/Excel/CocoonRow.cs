using KazusaHSR.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KazusaHSR.GameServer.Resource.Excel;

public class CocoonRow
{
	public uint Id { get; set; }
	public uint PropId { get; set; }
	public uint MappingInfoId { get; set; }
	public uint StageId { get; set; }
	public List<uint> StageIdList { get; set; }
	public List<uint> DropList { get; set; }
	public uint StaminaCost { get; set; }
	public uint? WorldLevel { get; set; }
}
