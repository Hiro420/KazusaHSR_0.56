using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class AdventurePlayerRow
{
	public uint Id { get; set; }
	public uint AvatarId { get; set; }
	public string PlayerName { get; set; }
	public string PlayerPrefabPath { get; set; }
	public string PlayerJsonPath { get; set; }
	public string DefaultAvatarHeadIconPath { get; set; }
	public List<uint> MazeSkillIdList { get; set; }
}