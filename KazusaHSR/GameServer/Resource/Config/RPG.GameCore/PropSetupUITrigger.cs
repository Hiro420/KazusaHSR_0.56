namespace KazusaHSR.GameServer.Resource;

public class PropSetupUITrigger : TaskConfig
{
	public bool TargetIsOwner { get; set; }
	public string UniqueName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public string ColliderRelativePath { get; set; }
	public bool DestroyAfterTriggered { get; set; }
	public bool DisableAfterTriggered { get; set; }
	public string ButtonIcon { get; set; }
	public TextID ButtonText { get; set; }
	public TaskConfig[] ButtonCallback { get; set; }
	public bool ForceInteractInDanger { get; set; }
}
