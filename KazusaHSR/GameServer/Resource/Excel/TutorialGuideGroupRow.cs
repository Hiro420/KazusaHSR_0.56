namespace KazusaHSR.GameServer.Resource.Excel;

public class TutorialGuideGroupRow
{
	public uint GroupID { get; set; }
	public uint[] TutorialGuideIDList { get; set; }
	public bool IsAutoShow { get; set; }
	public string MessageIconPath { get; set; }
	public TextID MessageText { get; set; }
	public uint RewardID { get; set; }
}
