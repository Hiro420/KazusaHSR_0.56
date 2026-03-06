namespace KazusaHSR.GameServer.Resource;

public class CreatePropInTargetFront : TaskConfig
{
	public TargetEvaluator Taskarget { get; set; }
	public uint PropID { get; set; }
	public string UniqueName { get; set; }
	public DynamicFloat Distance { get; set; }
	public DynamicFloat Duration { get; set; }
	public DynamicFloat NumLimitation { get; set; }
	public DynamicFloat Radius { get; set; }
	public TeamType Team { get; set; }
	public JNCHJAFHMMI CampID { get; set; }
	public TaskConfig[] OnCreate { get; set; }
	public TaskConfig[] OnDestroy { get; set; }
}
