namespace KazusaHSR.GameServer.Resource;

public class CharacterNavigateFollow : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public string TargetCharacterUniqueName { get; set; }
	public CharacterMotionFlag MotionFlag { get; set; }
	public float FollowDistance { get; set; }
	public bool WaitUntilFinish { get; set; }
	public bool AvoidOthers { get; set; }
}
