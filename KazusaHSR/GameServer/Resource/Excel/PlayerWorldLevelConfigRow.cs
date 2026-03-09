namespace KazusaHSR.GameServer.Resource.Excel;

public class PlayerWorldLevelConfigRow
{
	public uint Level { get; set; }
	public uint MaxPlayerLevel { get; set; }
	public uint LevelUpMission { get; set; }
	public TextID Breaktips1 { get; set; }
	public TextID Breaktips2 { get; set; }
	public TextID LevelUpMissionTips { get; set; }
}
