namespace KazusaHSR.GameServer.Resource;

public class ByRandomChance : PredicateConfig
{
	public DynamicFloat Chance { get; set; }
	public string LogComment { get; set; }
}
