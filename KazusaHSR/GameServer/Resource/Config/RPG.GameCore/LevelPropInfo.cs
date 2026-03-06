namespace KazusaHSR.GameServer.Resource;

public class LevelPropInfo : NamedLevelObjectInfo
{
	public float RotX { get; set; }
	public float RotZ { get; set; }
	public uint PropID { get; set; }
	public bool IsLimitedLife { get; set; }
	public float LifeTime { get; set; }
	public string InitLevelGraph { get; set; }
	public LevelTriggerInfo Trigger { get; set; }
	public bool CreateOnInitial { get; set; } = true;
	public LevelGraphValueSource ValueSource { get; set; }
	public uint CampID { get; set; }
	public PropState State { get; set; }
	public uint RecordID { get; set; }
	public uint EventID { get; set; }
	public uint AnchorGroupID { get; set; }
	public uint AnchorID { get; set; }
	public uint MapTeleportID { get; set; }
	public uint ChestID { get; set; }
	public uint DialogueTriggerAngle { get; set; }
	public uint[] DialogueGroups { get; set; }
	public bool OverrideTriggerHint { get; set; }
	public float HintRange { get; set; }
	public uint[] ServerInteractVerificationIDList { get; set; }
	public StageObjectCapture StageObjectCapture { get; set; }
}
