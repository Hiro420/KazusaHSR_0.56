namespace KazusaHSR.GameServer.Resource;

public class CharacterSteerTo : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public string AnchorName { get; set; }
	public string AreaName { get; set; }
	public float Duration { get; set; }
	public string TargetCharacterUniqueName { get; set; }
	public bool WaitUntilFinish { get; set; }
}
