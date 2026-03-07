using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class UniqueActorRow
{
	public string UniqueName { get; set; }
	public string ActorID { get; set; }
	public string[] HideMeshList { get; set; }
}
