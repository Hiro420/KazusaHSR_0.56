namespace KazusaHSR.GameServer.Resource;

public class ProjectileData
{
	public IGKGDKJCGHK Behavior { get; set; }
	public float FlySpeed { get; set; }
	public bool EnableRayCast { get; set; }
	public float FlyTime { get; set; }
	public float Gravity { get; set; }
	public float MaxLifeTime { get; set; }
	public float CollisionEnableDelay { get; set; }
	public bool StartFromLastHitPos { get; set; }
	public string AttachPoint { get; set; }
	public MVector3 AttachOffset { get; set; }
	public string TargetAttachPoint { get; set; }
	public MVector3 TargetOffset { get; set; }
	public float TargetDistanceOffset { get; set; }
	public float HitEffectDistanceOffset { get; set; }
	public bool IgnoreTargetHitbox { get; set; }
	public string FlyEffectUniqueName { get; set; }
	public string FlyEffect { get; set; }
	public string HitEffect { get; set; }
	public float LinearPitchAngle { get; set; }
	public float BoomerangEccentricity { get; set; }
	public float BoomerangAngleRoll { get; set; }
	public bool WriteProgressToEffectAnimator { get; set; }
	public bool TriggerHitCallback { get; set; }
	public MVector3 StartDirction { get; set; }
	public MVector3 StartOffsetRange { get; set; }
	public MVector3 TargetOffsetRange { get; set; }
	public float TraceDelay { get; set; }
	public float TurnSpeed { get; set; }
	public float ArriveDistance { get; set; }
}
