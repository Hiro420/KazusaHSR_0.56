using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ChapterRow
{
	public uint ChapterID { get; set; }
	public TextID ChapterName { get; set; }
	public TextID ChapterDesc { get; set; }
	public uint VolumeID { get; set; }
	public string ChapterPrefabPath { get; set; }
	public string ChapterPrefabName { get; set; }
	public uint ChapterCameraID { get; set; }
	public string ChapterIconPath { get; set; }
	public TextID ChapterTitle { get; set; }
}
