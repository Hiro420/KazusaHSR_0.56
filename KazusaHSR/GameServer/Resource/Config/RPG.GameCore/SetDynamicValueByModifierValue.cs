namespace KazusaHSR.GameServer.Resource;

public class SetDynamicValueByModifierValue : TaskConfig
{
	public TargetEvaluator ReadTargetType { get; set; }
	public string ModifierName { get; set; }
	public OEMCEJMAHHF ValueType { get; set; }
	public DynamicFloat[] Lookup { get; set; }
	public DynamicFloat Multiplier { get; set; }
	public string DynamicKey { get; set; }
}
