namespace KazusaHSR.GameServer.Resource.Excel;

public class TutorialRow
{
	public uint TutorialId { get; set; }
	public string TutorialJsonPath { get; set; }
	public List<TriggerParam> TriggerParams { get; set; }
}