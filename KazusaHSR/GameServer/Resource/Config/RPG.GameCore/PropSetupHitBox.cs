namespace KazusaHSR.GameServer.Resource;

public class PropSetupHitBox : TaskConfig
{
	public string UniqueName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public string ColliderRelativePath { get; set; }
	public TaskConfig[] OnBeHit { get; set; }
}
