namespace KazusaHSR.GameServer.Resource;

public class SwitchCharacterAnchor : TaskConfig
{
	public bool IsLocalPlayer { get; set; }
	public string CharacterUniqueName { get; set; }
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
	public bool ResetAnimation { get; set; }
}
