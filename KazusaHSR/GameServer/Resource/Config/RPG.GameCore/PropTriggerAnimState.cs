namespace KazusaHSR.GameServer.Resource;

public class PropTriggerAnimState : TaskConfig
{
	public bool TargetIsOwner { get; set; }
	public uint GroupPropID { get; set; }
	public uint GroupID { get; set; }
	public float NormalizedTimeStart { get; set; }
	public float TransitionDuration { get; set; }
	public bool FixedTransitionDuration { get; set; }
	public string AnimStateName { get; set; }
}
