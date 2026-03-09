namespace KazusaHSR.GameServer.Resource.Excel;

public class MapGuideRow
{
	public uint ID { get; set; }
	public uint WorldID { get; set; }
	public TextID MapGuideName { get; set; }
	public uint SheetID { get; set; }
	public uint SheetType { get; set; }
	public string MapGuideIconPath { get; set; }
}
