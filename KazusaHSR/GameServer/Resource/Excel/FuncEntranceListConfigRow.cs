namespace KazusaHSR.GameServer.Resource.Excel;

public class FuncEntranceListConfigRow
{
	public uint PlaneType { get; set; }
	public uint SubType { get; set; }
	public uint[] FuncEntranceIDList { get; set; }
	public uint[] BottomFuncEntranceIDList { get; set; }
	public uint[] HudFuncEntranceIDList { get; set; }
	public uint[] UnlockGotoTypeList { get; set; }
}
