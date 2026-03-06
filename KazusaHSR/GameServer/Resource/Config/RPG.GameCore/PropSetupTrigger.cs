namespace KazusaHSR.GameServer.Resource;

public class PropSetupTrigger : TaskConfig
{
	public string UniqueName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public bool IsSingle { get; set; }
	public CharacterType TargetCharacterType { get; set; }
	public uint TargetGroupID { get; set; }
	public uint TargetID { get; set; }
	public CharacterType[] TargetTypes { get; set; }
	public string ColliderRelativePath { get; set; }
	public bool DestroyAfterTriggered { get; set; }
	public bool DisableAfterTriggered { get; set; }
	public TaskConfig[] OnTriggerEnter { get; set; }
	public TaskConfig[] OnTriggerExit { get; set; }
}
