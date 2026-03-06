namespace KazusaHSR.GameServer.Resource;

public class CreateBattleEventEntity : TaskConfig
{
	public NAFOHGFPIJB EventType { get; set; }
	public FixPoint Speed { get; set; }
	public string[] AbilityList { get; set; }
	public uint WarningTurnLeft { get; set; }
	public string IconPath { get; set; }
}
