using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AdventurePlayerRow
{
	public uint ID { get; set; }
	public uint AvatarID { get; set; }
	public string PlayerName { get; set; }
	public string PlayerPrefabPath { get; set; }
	public string PlayerJsonPath { get; set; }
	public string DefaultAvatarHeadIconPath { get; set; }
	public uint[] MazeSkillIdList { get; set; }
}
