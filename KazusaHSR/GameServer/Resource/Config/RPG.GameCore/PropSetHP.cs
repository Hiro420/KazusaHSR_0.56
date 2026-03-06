namespace KazusaHSR.GameServer.Resource;

public class PropSetHP : TaskConfig
{
	public string UniqueName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public FixPoint HP { get; set; }
	public FixPoint MaxHP { get; set; }
}
