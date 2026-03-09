namespace KazusaHSR.GameServer.Resource.Excel;

public class DocumentaryPhaseQuestPanelRow
{
	public uint PhaseID { get; set; }
	public uint NextPhase { get; set; }
	public uint[] QuestList { get; set; }
	public TextID PanelTitle { get; set; }
	public TextID PanelDesc { get; set; }
}
