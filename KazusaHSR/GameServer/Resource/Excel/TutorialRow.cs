using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class TutorialRow
{
	public uint TutorialId { get; set; }
	public string TutorialJsonPath { get; set; }
	public List<TriggerParam> TriggerParams { get; set; }
}