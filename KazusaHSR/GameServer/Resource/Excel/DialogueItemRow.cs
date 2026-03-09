namespace KazusaHSR.GameServer.Resource.Excel;

public class DialogueItemRow
{
	public uint ID { get; set; }
	public uint NextDialogueID { get; set; }
	public uint BelongedGroupID { get; set; }
	public uint VoiceID { get; set; }
	public string ActPath { get; set; }
}
