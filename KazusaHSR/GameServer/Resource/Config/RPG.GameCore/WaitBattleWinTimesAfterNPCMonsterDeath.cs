namespace KazusaHSR.GameServer.Resource;

public class WaitBattleWinTimesAfterNPCMonsterDeath : TaskConfig
{
	public uint GroupID { get; set; }
	public uint GroupMonsterID { get; set; }
	public string UniqueName { get; set; }
	public int WinTimes { get; set; }
}
