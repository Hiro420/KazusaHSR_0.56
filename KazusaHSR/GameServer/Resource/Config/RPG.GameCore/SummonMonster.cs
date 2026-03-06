namespace KazusaHSR.GameServer.Resource;

public class SummonMonster : TaskConfig
{
	public SummonMonsterData[] SummonMonsterDataList { get; set; }
	public VCameraConfig CameraConfig { get; set; }
	public DynamicFloat DelayRatio { get; set; }
	public bool InheritSummonerDelay { get; set; }
	public bool RefreshTeamLocation { get; set; }
	public DynamicFloat InitHP { get; set; }
}
