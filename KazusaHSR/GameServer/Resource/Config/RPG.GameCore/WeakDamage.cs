namespace KazusaHSR.GameServer.Resource;

public class WeakDamage : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public DynamicFloat WeakDamageValue { get; set; }
	public DynamicFloat UnWeakDamageValue { get; set; }
}
