namespace KazusaHSR.GameServer.Resource;

public class ByCompareWaveCount : PredicateConfig
{
	public LAMJJAFBOBM CompareType { get; set; }
	public bool CompareWithMax { get; set; }
	public DynamicFloat CompareValue { get; set; }
}
