namespace KazusaHSR.GameServer.Resource;

public class MoveStageOnTargetForward : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public float StageRootOffset { get; set; }
}
