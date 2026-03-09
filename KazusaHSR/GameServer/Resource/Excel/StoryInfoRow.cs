namespace KazusaHSR.GameServer.Resource.Excel;

public class StoryInfoRow
{
	public uint StoryID { get; set; }
	public string StoryPath { get; set; }
	public bool IsSkip { get; set; }
	public string SoundBank { get; set; }
	public FMBPFAAKKMC LevelType { get; set; }
}
