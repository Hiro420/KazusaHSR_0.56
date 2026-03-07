using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class WorldLevelRow
{
	public uint Level { get; set; }
	public uint MaxPlayerLevel { get; set; }
	public uint LevelUpMission { get; set; }
	public TextID Breaktips1 { get; set; }
	public TextID Breaktips2 { get; set; }
	public TextID LevelUpMissionTips { get; set; }
}
