namespace KazusaHSR.GameServer.Resource;

public class ByCheckTurnCountMatch : PredicateConfig
{
	public uint RoundCount { get; set; }
	public TeamType TeamType { get; set; }
	public uint TargetID { get; set; }
	public EIKJAFNEMOM OverrideConfig { get; set; }
}
