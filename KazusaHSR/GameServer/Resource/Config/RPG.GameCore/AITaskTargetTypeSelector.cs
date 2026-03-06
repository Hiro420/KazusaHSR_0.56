namespace KazusaHSR.GameServer.Resource;

public class AITaskTargetTypeSelector : AISelector
{
	public TargetEvaluator TargetType { get; set; }
	public bool InverseResultFlag { get; set; }
}
