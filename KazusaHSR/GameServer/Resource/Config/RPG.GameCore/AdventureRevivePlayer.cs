namespace KazusaHSR.GameServer.Resource;

public class AdventureRevivePlayer : TaskConfig
{
	public DynamicFloat RevivedHPRatio { get; set; }
	public DynamicFloat RevivedSPRatio { get; set; }
	public TargetEvaluator SpecifyTargetType { get; set; }
	public Dictionary<string, TargetEvaluator> TargetType { get; set; }
	public bool ClientUseGMCommand { get; set; }
}
