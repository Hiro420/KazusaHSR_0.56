namespace KazusaHSR.GameServer.Resource;

public class ToastPage : TaskConfig
{
	public ToastType Type { get; set; }
	public TextID MessageOne { get; set; }
	public TextID MessageTwo { get; set; }
}
