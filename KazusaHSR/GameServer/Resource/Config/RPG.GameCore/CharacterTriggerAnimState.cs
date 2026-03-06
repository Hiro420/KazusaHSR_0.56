namespace KazusaHSR.GameServer.Resource;

public class CharacterTriggerAnimState : TaskConfig
{
	public CharacterType CharType { get; set; }
	public string CharacterUniqueName { get; set; }
	public bool ChangeAnimatorCatgory { get; set; }
	public NDCCEODGDCK Catgory { get; set; }
	public bool AutoExitToDefault { get; set; }
	public bool UseMotionID { get; set; }
	public string AnimStateName { get; set; }
	public string StoryAvatarID { get; set; }
	public string StartMotion { get; set; }
	public float NormalizedTimeStart { get; set; }
	public float NormalizedTransitionDuration { get; set; }
	public float NormalizedTimeWait { get; set; }
	public uint StoryMotionID { get; set; }
}
