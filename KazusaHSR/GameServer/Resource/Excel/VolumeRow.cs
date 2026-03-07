using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class VolumeRow
{
	public uint VolumeID { get; set; }
	public TextID VolumeTitleName { get; set; }
	public TextID VolumeName { get; set; }
	public TextID VolumeDesc { get; set; }
	public string VolumeIconPath { get; set; }
}
