namespace KazusaHSR.GameServer.Resource.Excel;

public class InventoryTabRow
{
	public uint ID { get; set; }
	public TextID TabName { get; set; }
	public string IconImagePath { get; set; }
	public ItemType[] DisplayItemType { get; set; }
	public uint TabSortWeight { get; set; }
	public InventorySortType[] ItemSortTypeList { get; set; }
	public uint DisplayCapacityLimit { get; set; }
}
