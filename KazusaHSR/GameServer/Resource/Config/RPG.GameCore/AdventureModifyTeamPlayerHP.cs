namespace KazusaHSR.GameServer.Resource;

public class AdventureModifyTeamPlayerHP : TaskConfig
{
	public DynamicFloat AddRatio { get; set; }
	public IGNHFEOEBHO ModifyFunction { get; set; }
	public DynamicFloat ModifyValue { get; set; }
	public TargetEvaluator SpecifyTargetType { get; set; }
	public bool ClientUseGMCommand { get; set; }
}
