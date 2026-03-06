namespace KazusaHSR.GameServer.Resource;

public class CaptureNPCToCharacter : TaskConfig
{
	public uint GroupID { get; set; }
	public uint GroupNpcID { get; set; }
	public string CharacterUniqueName { get; set; }
}
