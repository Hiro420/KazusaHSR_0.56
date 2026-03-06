namespace KazusaHSR.GameServer.Resource;

public class EnableBattleOnAttacked : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool Enabled { get; set; }
}
