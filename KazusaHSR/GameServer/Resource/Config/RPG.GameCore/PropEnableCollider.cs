namespace KazusaHSR.GameServer.Resource;

public class PropEnableCollider : TaskConfig
{
	public string UniqueName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public bool Enabled { get; set; }
	public string[] SpecifiedRelativePaths { get; set; }
}
