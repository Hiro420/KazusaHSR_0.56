namespace KazusaHSR.GameServer.Resource;

public class AddCustomValue : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string Key { get; set; }
	public DynamicFloat AddValue { get; set; }
	public DynamicFloat Min { get; set; }
	public DynamicFloat Max { get; set; }
}
