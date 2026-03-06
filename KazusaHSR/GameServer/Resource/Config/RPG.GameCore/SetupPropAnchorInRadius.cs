namespace KazusaHSR.GameServer.Resource;

public class SetupPropAnchorInRadius : TaskConfig
{
	public string AnchorName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public float AnchorRotOffset { get; set; }
	public string CentreAnchorArea { get; set; }
	public string CentreAnchorName { get; set; }
	public float RadiusMin { get; set; }
	public float RadiusMax { get; set; }
	public float AngleMin { get; set; }
	public float AngleMax { get; set; }
}
