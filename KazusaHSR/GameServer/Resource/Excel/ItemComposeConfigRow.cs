namespace KazusaHSR.GameServer.Resource.Excel;

public class ItemComposeConfigRow
{
	public uint ID { get; set; }
	public uint ItemID { get; set; }
	public ItemConfig[] MaterialCost { get; set; }
	public uint CoinCost { get; set; }
	public uint Type { get; set; }
	public uint Order { get; set; }
	public uint WorldLevelRequire { get; set; }
}
