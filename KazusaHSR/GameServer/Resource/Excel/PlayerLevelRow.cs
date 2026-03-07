using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class PlayerLevelRow
{
	public uint Level { get; set; }
	public uint PlayerExp { get; set; }
	public uint StaminaLimit { get; set; }
	public uint RewardStamina { get; set; }
	public uint RewardID { get; set; }
	public uint LevelRewardID { get; set; }
}
