namespace KazusaHSR.GameServer.Resource.Excel;

public class StaminaExchangeRow
{
	public uint Times { get; set; }
	public Dictionary<uint, uint> Price { get; set; }
	public uint ToStamina { get; set; }
}
