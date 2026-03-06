namespace KazusaHSR.GameServer.Resource;

public class SetDynamicValueByStatusCount : TaskConfig
{
	public string DynamicKey { get; set; }
	public TargetEvaluator ReadTargetType { get; set; }
	public DFNFPENLIBD StatusType { get; set; }
}
