namespace KazusaHSR.GameServer.Resource.Excel;

public class TutorialDataRow
{
	public uint TutorialID { get; set; }
	public string TutorialJsonPath { get; set; }
	public JEKKEGPBBLA SaveType { get; set; }
	public TutorialTriggerParam[] TriggerParams { get; set; }
}
