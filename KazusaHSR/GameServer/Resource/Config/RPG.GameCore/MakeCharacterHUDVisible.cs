namespace KazusaHSR.GameServer.Resource;

public class MakeCharacterHUDVisible : TaskConfig
{
	public TargetEvaluator TargetType { get; set; }
	public bool Visible { get; set; }
}
