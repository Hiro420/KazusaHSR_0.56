using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class TeamBuffRow
{
	public uint TeamBuffID { get; set; }
	public TextID TeamBuffName { get; set; }
	public string IconPath { get; set; }
	public TextID TeamBuffDesc { get; set; }
	public string DescModifier { get; set; }
	public uint BPCost { get; set; }
	public string EntryAbility { get; set; }
}
