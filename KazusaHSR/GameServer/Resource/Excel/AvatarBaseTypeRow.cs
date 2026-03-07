using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarBaseTypeRow
{
	public AvatarBaseType ID { get; set; }
	public string BaseTypeIcon { get; set; }
	public string BaseTypeIconSmall { get; set; }
	public string EquipmentLightMatPath { get; set; }
	public TextID BaseTypeText { get; set; }
}
