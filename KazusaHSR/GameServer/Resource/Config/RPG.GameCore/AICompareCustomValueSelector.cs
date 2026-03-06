namespace KazusaHSR.GameServer.Resource;

public class AICompareCustomValueSelector : AISelector
{
	public string CustomValueKey { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public int CompareTarget { get; set; }
	public bool InverseResultFlag { get; set; }
}
