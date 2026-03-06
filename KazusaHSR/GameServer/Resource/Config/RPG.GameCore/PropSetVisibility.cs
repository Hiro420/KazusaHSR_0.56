namespace KazusaHSR.GameServer.Resource;

public class PropSetVisibility : TaskConfig
{
	public string UniqueName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public bool Visible { get; set; }
	public string[] SpecifiedRelativePaths { get; set; }
}
