namespace KazusaHSR.GameServer.Resource;

public class CharacterDisableLookAt : TaskConfig
{
	public bool TargetIsOwner { get; set; }
	public string CharacterUniqueName { get; set; }
	public bool Disable { get; set; }
}
