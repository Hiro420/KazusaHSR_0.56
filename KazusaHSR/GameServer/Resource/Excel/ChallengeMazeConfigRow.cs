using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ChallengeMazeConfigRow
{
	public uint ID { get; set; }
	public TextID Name { get; set; }
	public uint MapEntranceID { get; set; }
	public uint PreMissionID { get; set; }
	public uint PreLevel { get; set; }
	public uint PreChallengeMazeID { get; set; }
	public uint RewardID { get; set; }
	public uint[] MonsterID { get; set; }
	public uint[] StageID { get; set; }
	public AttackDamageType[] DamageType { get; set; }
	public uint ProposeLevel { get; set; }
	public uint[] MazeBuff { get; set; }
	public uint[] ChallengeTargetID { get; set; }
}
