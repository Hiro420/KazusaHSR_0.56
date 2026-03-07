using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class GotoConfigRow
{
	public uint ID { get; set; }
	public uint GotoType { get; set; }
	public uint[] ParamIntList { get; set; }
	public string[] ParamStringList { get; set; }
	public uint UnlockMainMission { get; set; }
	public uint UnlockID { get; set; }
}
