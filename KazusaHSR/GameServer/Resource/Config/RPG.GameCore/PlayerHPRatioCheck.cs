namespace KazusaHSR.GameServer.Resource;

public class PlayerHPRatioCheck : TaskConfig
{
	public uint AvatarID { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public FixPoint TargetHPRatio { get; set; }
}
