namespace KazusaHSR.GameServer.Resource;

public class AdvForceSetNpcAlertValue : TaskConfig
{
	public uint GroupID { get; set; }
	public uint NpcMonsterID { get; set; }
	public float AlertValue { get; set; }
}
