namespace KazusaHSR.GameServer.Resource;

public class SetModifierValue : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public string ModifierName { get; set; }
	public IGNHFEOEBHO ModifyFunction { get; set; }
	public OEMCEJMAHHF ValueType { get; set; }
	public DynamicFloat Value { get; set; }
}
