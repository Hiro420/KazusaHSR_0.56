namespace KazusaHSR.GameServer.Resource;

public class CharacterNavigateTo : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
	public CharacterMotionFlag MotionFlag { get; set; }
	public bool WaitUntilFinish { get; set; }
	public float Duration { get; set; }
	public bool AvoidOthers { get; set; }
}
