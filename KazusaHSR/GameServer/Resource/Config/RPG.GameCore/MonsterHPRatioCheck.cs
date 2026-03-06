namespace KazusaHSR.GameServer.Resource;

public class MonsterHPRatioCheck : TaskConfig
{
	public string[] MonsterTagList { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public FixPoint[] TargetHPRatioList { get; set; }
}
