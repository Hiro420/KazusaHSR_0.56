namespace KazusaHSR.GameServer.Resource;

public class AbilityPropertyRangeGroup
{
	public AbilityProperty Property { get; set; }
	public DynamicValueRangeCallback[] Ranges { get; set; }
}
