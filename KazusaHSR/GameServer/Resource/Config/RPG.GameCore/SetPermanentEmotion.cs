namespace KazusaHSR.GameServer.Resource;

public class SetPermanentEmotion : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public string BrowEmotion { get; set; }
	public string EyeEmotion { get; set; }
	public string MouthEmotion { get; set; }
}
