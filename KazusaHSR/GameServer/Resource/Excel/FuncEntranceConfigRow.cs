using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class FuncEntranceConfigRow
{
	public uint ID { get; set; }
	public TextID FuncName { get; set; }
	public string FuncIconPath { get; set; }
	public string FuncHudIconPath { get; set; }
	public uint GotoID { get; set; }
	public uint UnlockMainMission { get; set; }
	public uint UnlockID { get; set; }
	public TextID UnlockDesc { get; set; }
	public string RedDot { get; set; }
	public string RedDotHud { get; set; }
}
