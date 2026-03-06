namespace KazusaHSR.GameServer.Resource;

public class ResetCustomValue : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string Key { get; set; }
	public DynamicFloat ResetValue { get; set; }
}
