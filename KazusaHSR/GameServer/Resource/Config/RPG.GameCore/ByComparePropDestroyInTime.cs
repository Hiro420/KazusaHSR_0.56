namespace KazusaHSR.GameServer.Resource;

public class ByComparePropDestroyInTime : PredicateConfig
{
	public string UniqueName { get; set; }
	public float AfterSeconds { get; set; }
	public uint GroupID { get; set; }
	public uint GroupPropID { get; set; }
}
