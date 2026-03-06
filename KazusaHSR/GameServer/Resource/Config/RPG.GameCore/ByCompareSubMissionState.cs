namespace KazusaHSR.GameServer.Resource;

public class ByCompareSubMissionState : PredicateConfig
{
	public uint SubMissionID { get; set; }
	public SubMissionState SubMissionState { get; set; }
}
