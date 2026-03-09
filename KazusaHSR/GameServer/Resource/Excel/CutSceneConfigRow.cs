namespace KazusaHSR.GameServer.Resource.Excel;

public class CutSceneConfigRow
{
	public string CutSceneName { get; set; }
	public string CutScenePath { get; set; }
	public string SoundBank { get; set; }
	public string CutSceneBGMStateName { get; set; }
	public float[] PosOffSet { get; set; }
	public uint MazePlaneID { get; set; }
	public uint MazeFloorID { get; set; }
}
