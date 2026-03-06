namespace KazusaHSR.GameServer.Resource;

public class PropSetupOptionsTrigger : TaskConfig
{
	public bool TargetIsOwner { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public bool DestroyAfterTriggered { get; set; }
	public bool DisableAfterTriggered { get; set; }
	public OptionTriggerInfo[] OptionList { get; set; }
}
