using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class GameplayGuideTabConfigRow
{
	public uint ID { get; set; }
	public TextID Name { get; set; }
	public string IconPath { get; set; }
	public PanelType PanelType { get; set; }
	public uint[] GameplayGuideDataID { get; set; }
}
