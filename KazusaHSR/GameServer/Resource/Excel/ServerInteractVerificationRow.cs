using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ServerInteractVerificationRow
{
	public uint ID { get; set; }
	public MKDFAMENEFB InteractType { get; set; }
	public uint[] InteractTypeConfig { get; set; }
	public uint WorldLevelRequire { get; set; }
	public uint MissionRequire { get; set; }
}
