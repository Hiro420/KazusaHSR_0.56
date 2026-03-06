namespace KazusaHSR.GameServer.Resource;

public class ShowBattleMessage : TaskConfig
{
	public TextID ContentID { get; set; }
	public float LifetimeNormal { get; set; }
	public float LifetimeMin { get; set; }
	public bool WaitShowPageFinish { get; set; }
}
