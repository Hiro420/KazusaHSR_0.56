namespace KazusaHSR.GameServer.Resource;

public class WaitSecondsAfterNPCMonsterDeath : TaskConfig
{
	public uint GroupID { get; set; }
	public uint GroupMonsterID { get; set; }
	public string UniqueName { get; set; }
	public float WaitTime { get; set; }
}
