namespace KazusaHSR.GameServer.Resource;

public class SummonBattleEventMonster : TaskConfig
{
	public SummonMonsterData MonsterData { get; set; }
	public VCameraConfig CameraConfig { get; set; }
	public DynamicFloat DelayRatio { get; set; }
	public bool InheritSummonerDelay { get; set; }
	public string AnchorName { get; set; }
}
