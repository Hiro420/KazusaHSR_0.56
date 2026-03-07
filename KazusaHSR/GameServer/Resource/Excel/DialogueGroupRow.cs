using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class DialogueGroupRow
{
	public uint GroupID { get; set; }
	public BDAHFPGHLMB GroupType { get; set; }
	public uint StartDialogueID { get; set; }
	public uint EndDialogueID { get; set; }
	public string InteractTitle { get; set; }
	public uint[] ConditionIDs { get; set; }
	public uint Priority { get; set; }
	public bool IsShowPlayer { get; set; }
	public string CameraPath { get; set; }
}
