using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ExtraEffectRow
{
	public uint ExtraEffectID { get; set; }
	public TextID ExtraEffectName { get; set; }
	public TextID ExtraEffectDesc { get; set; }
	public string ExtraEffectIconPath { get; set; }
	public uint ExtraEffectType { get; set; }
}
