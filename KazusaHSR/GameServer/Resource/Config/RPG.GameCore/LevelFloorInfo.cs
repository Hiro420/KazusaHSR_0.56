namespace KazusaHSR.GameServer.Resource;

public class LevelFloorInfo
{
	public uint FloorID { get; set; }
	public string FloorName { get; set; }
	public string SceneName { get; set; }
	public uint StartGroupID { get; set; }
	public uint StartAnchorID { get; set; }
	public string LevelGraph { get; set; }
	public LevelGroupInstanceInfo[] GroupList { get; set; }
	public uint[] DefaultGroupIDList { get; set; }
	public uint[] UnlockMainMissionGroupIDList { get; set; }
	public string EnviroProfile { get; set; }
	public string StageData { get; set; }
	public string NavMesh { get; set; }
	public string MinimapVolume { get; set; }
	public NIDMCIHPJEL CameraType { get; set; }
	public LevelMinimapVolume MinimapVolumeData { get; set; }
}
