namespace KazusaHSR.GameServer.Resource;

public class AIPropertySelector : AISelector
{
	public EMMHJEBMGMJ PropertyStrategy { get; set; }
	public AbilityProperty Property { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public FixPoint CompareValue { get; set; }
	public FixPoint CompareRatio { get; set; }
	public bool InverseResultFlag { get; set; }
}
