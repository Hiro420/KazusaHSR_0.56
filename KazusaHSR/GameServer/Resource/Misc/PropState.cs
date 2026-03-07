using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource.Excel;

public enum PropState
{
	Closed = 0,
	Open = 1,
	Locked = 2,
	BridgeState1 = 3,
	BridgeState2 = 4,
	BridgeState3 = 5,
	BridgeState4 = 6,
	CheckPointDisable = 7,
	CheckPointEnable = 8,
	TriggerDisable = 9,
	TriggerEnable = 10,
	ChestLocked = 11,
	ChestClosed = 12,
	ChestUsed = 13,
	Elevator1 = 14,
	Elevator2 = 15,
	Elevator3 = 16,
}
