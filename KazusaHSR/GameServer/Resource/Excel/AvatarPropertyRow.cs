using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AvatarPropertyRow
{
	public AvatarPropertyType PropertyType { get; set; }
	public TextID PropertyName { get; set; }
	public bool IsDisplay { get; set; }
	public bool isBattleDisplay { get; set; }
	public uint Order { get; set; }
	public string IconPath { get; set; }
}
