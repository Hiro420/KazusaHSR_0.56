using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ActivityPanelRow
{
	public uint PanelID { get; set; }
	public uint Type { get; set; }
	public uint[] TypeParam { get; set; }
	public string BeginTime { get; set; }
	public string EndTime { get; set; }
	public uint MinPlayerLevel { get; set; }
	public uint MaxPlayerLevel { get; set; }
	public bool IsShowDayTime { get; set; }
	public uint SortWeight { get; set; }
	public TextID TabName { get; set; }
	public TextID TitleName { get; set; }
	public TextID PanelDesc { get; set; }
	public string GraphImagePath { get; set; }
	public string BGImagePath { get; set; }
}
