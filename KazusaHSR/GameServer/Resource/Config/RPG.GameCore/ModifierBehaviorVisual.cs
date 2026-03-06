namespace KazusaHSR.GameServer.Resource;

public class ModifierBehaviorVisual
{
	public IFHKEKLGEFL AnimLoopCat { get; set; }
	public IFHKEKLGEFL AnimOccurCat { get; set; }
	public IFHKEKLGEFL AnimRecoverCat { get; set; }
	public int AnimLoopHash { get; set; }
	public int AnimOccurHash { get; set; }
	public int AnimRecoverHash { get; set; }
	public int FloatingFailAnimRecoverHash { get; set; }
	public HitMotionParams FloatingParams { get; set; }
	public BEAPKPIOEHJ Flag { get; set; }
	public float Priority { get; set; }
	public string AnimOccur { get; set; }
	public string AnimLoop { get; set; }
	public string AnimRecover { get; set; }
	public float AnimOccurTransition { get; set; }
	public float AnimLoopTransition { get; set; }
	public float AnimRecoverTransition { get; set; }
	public float AnimSpeedFactor { get; set; }
	public float AnimSpeedTransitTime { get; set; }
	public float AnimSpeedTransitTimeRange { get; set; }
	public bool ForecHitH { get; set; }
	public CharacterReactionAnimConfig ReactionAnims { get; set; }
	public ACGPOEMCOJJ MaterialEffect { get; set; }
	public float MaterialTransitTime { get; set; }
	public float FloatingHeight { get; set; }
	public float FloatingRiseDuration { get; set; }
	public float FloatingFallDuration { get; set; }
	public string FloatingFailAnimRecover { get; set; }
	public bool DisableLookAtIK { get; set; }
}
