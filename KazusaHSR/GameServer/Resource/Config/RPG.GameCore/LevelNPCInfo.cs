namespace KazusaHSR.GameServer.Resource;

public class LevelNPCInfo : NamedLevelObjectInfo
{
	public uint NPCID { get; set; }
	public string LevelGraph { get; set; }
	public bool CreateOnInitial { get; set; } = true;
	public LevelGraphValueSource ValueSource { get; set; }
	public uint CampID { get; set; }
	public LevelTriggerInfo Trigger { get; set; }
	public AIConfigInfo AIConfig { get; set; }
	public uint RecordID { get; set; }
	public uint EventID { get; set; }
	public bool Stable { get; set; }
	public uint DialogueTriggerAngle { get; set; }
	public uint[] DialogueGroups { get; set; }
	public uint[] ServerInteractVerificationIDList { get; set; }
	public string DefaultIdleStateName { get; set; }
}
