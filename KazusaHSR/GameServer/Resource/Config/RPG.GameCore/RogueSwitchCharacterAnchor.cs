namespace KazusaHSR.GameServer.Resource;

public class RogueSwitchCharacterAnchor : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public DynamicFloat TeleportIdx { get; set; }
}
