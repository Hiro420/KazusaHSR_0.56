using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class MiniMapIconRow
{
	public uint ID { get; set; }
	public string IconPath { get; set; }
	public bool IconOrientetionSwitch { get; set; }
	public string IconName { get; set; }
	public bool isShowinMap { get; set; }
	public bool IsShowInBillboard { get; set; }
	public string BillboardIconPath { get; set; }
	public string MissionIconPath { get; set; }
	public uint ConnectID { get; set; }
}
