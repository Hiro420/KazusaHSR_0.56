namespace KazusaHSR.GameServer.Resource;

public class CharacterHeadLookAt : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public string TargetCharacterUniqueName { get; set; }
	public string TargetCharacterAttachPoint { get; set; }
	public string TargetAreaName { get; set; }
	public string TargetAnchorName { get; set; }
	public bool KeepTracking { get; set; }
	public float Duration { get; set; }
	public string CurveName { get; set; }
	public CharacterHeadConstraint Constraint { get; set; }
	public bool WaitUntilFinish { get; set; }
}
