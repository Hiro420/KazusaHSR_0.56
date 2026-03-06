namespace KazusaHSR.GameServer.Resource;

public class ByComparePropStateNumber : PredicateConfig
{
	public uint GroupID { get; set; }
	public uint[] PropIDList { get; set; }
	public PropState State { get; set; }
	public LAMJJAFBOBM CompareType { get; set; }
	public uint CompareValue { get; set; }
}
