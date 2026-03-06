namespace KazusaHSR.GameServer.Resource;

public class TryTriggerAid : TaskConfig
{
	public FCBFHGIEKKA AidType { get; set; }
	public DynamicFloat Probility { get; set; }
	public DynamicFloat LimitCountPerTurn { get; set; }
}
