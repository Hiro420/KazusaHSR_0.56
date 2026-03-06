namespace KazusaHSR.GameServer.Resource;

public class PlayScreenTransfer : TaskConfig
{
	public ScreenTransferType Type { get; set; }
	public ScreenTransferMode Mode { get; set; }
	public float CustomTime { get; set; }
	public bool KeepDisplay { get; set; }
}
