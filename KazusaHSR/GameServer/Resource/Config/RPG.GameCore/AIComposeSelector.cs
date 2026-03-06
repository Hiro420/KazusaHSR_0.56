namespace KazusaHSR.GameServer.Resource;

public class AIComposeSelector : AISelector
{
	public POKNBLFMGAL ComposeType { get; set; }
	public AISelector[] SelectorList { get; set; }
}
