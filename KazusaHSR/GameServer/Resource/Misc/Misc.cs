using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaHSR.GameServer.Resource;

public class TextID
{
	public long hash { get; set; }
}

public class FixPoint
{
	public float Value { get; set; }
}

public class TriggerParam
{
	public string TriggerType { get; set; }
	public string TriggerParamTriggerParam { get; set; }
}

public class ItemCost
{
	public uint ItemID { get; set; }
	public uint ItemNum { get; set; }
}

public enum MapEntryType
{
	Unknown = 0,
	Town = 1,
	Mission = 2,
	Explore = 3
}