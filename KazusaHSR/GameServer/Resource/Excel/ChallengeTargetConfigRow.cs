using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public class ChallengeTargetConfigRow
{
	public uint ID { get; set; }
	public ChallengeType ChallengeTargetType { get; set; }
	public TextID ChallengeTargetName { get; set; }
	public uint ChallengeTargetParam { get; set; }
	public uint RewardID { get; set; }
}
