using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public enum TutorialTriggerType
{
	None = 0,
	TutorialFinish = 1,
	GetItem = 2,
	AnyAvatarToLevel = 3,
	GetAvatar = 4,
	FinishMainMission = 5,
	TaskUnlock = 6,
	TakeSubMission = 7,
	EnterBattle = 8,
	GetAnyLightCone = 9,
}
