namespace KazusaHSR.GameServer.Resource;

public class RemoveMazeBuff : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public uint ID { get; set; }
}
