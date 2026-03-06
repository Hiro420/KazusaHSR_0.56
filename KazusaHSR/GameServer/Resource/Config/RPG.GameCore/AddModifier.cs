namespace KazusaHSR.GameServer.Resource;

public class AddModifier : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool AliveOnly { get; set; }
	public string ModifierName { get; set; }
	public bool LifeStepImmediately { get; set; }
	public DynamicFloat Chance { get; set; }
	public DynamicFloat LifeTime { get; set; }
	public DynamicFloat Count { get; set; }
	public DynamicFloat MaxLayer { get; set; }
	public Dictionary<string, DynamicFloat> DynamicValues { get; set; }
	public bool InheritCaster { get; set; }
	public TaskConfig[] SuccessTaskList { get; set; }
	public TaskConfig[] FailTaskList { get; set; }
	public TaskConfig[] ResistedTaskList { get; set; }
}
