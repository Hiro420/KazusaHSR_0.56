namespace KazusaHSR.GameServer.Resource;

public class AnimSetParameter : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string ParameterName { get; set; }
	public bool BoolValue { get; set; }
	public bool IsTrigger { get; set; }
	public float FloatValue { get; set; }
	public int IntegerValue { get; set; }
}
