namespace KazusaHSR.GameServer.Resource;

public class SetEntityVisiable : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool Visiable { get; set; }
	public bool MuteColliderWhenInvisiable { get; set; }
}
