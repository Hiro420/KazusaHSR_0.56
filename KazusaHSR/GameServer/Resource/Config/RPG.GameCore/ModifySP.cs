namespace KazusaHSR.GameServer.Resource;

public class ModifySP : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public IGNHFEOEBHO ModifyFunction { get; set; }
	public DynamicFloat ModifyValue { get; set; }
}
