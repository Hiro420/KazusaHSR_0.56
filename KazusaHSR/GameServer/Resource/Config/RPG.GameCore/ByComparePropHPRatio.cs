namespace KazusaHSR.GameServer.Resource;

public class ByComparePropHPRatio : PredicateConfig
{
	public string UniqueName { get; set; }
	public uint GroupID { get; set; }
	public uint GroupPropID { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public FixPoint CompareValue { get; set; }
}
