namespace KazusaHSR.GameServer.Resource;

public class AttackData
{
	public AttackDamageType DamageType { get; set; }
	public KFGLJDBFPFI FormulaType { get; set; }
	public DynamicFloat DamagePercentage { get; set; }
	public DynamicFloat DamageValue { get; set; }
	public DynamicFloat StanceValue { get; set; }
	public DynamicFloat HitSplitRatio { get; set; }
	public bool IsIndirect { get; set; }
	public DynamicFloat SPHitRatio { get; set; }
	public AttackDamageType StanceDamageType { get; set; }
	public AttackType AttackType { get; set; }
	public DynamicFloat FrameHalt { get; set; }
	public bool IsFaceToHitDir { get; set; }
	public string HitAnimation { get; set; }
	public string HitEffect { get; set; }
	public float HitEffectHeight { get; set; }
	public float HitEffectOffsetAngle { get; set; }
	public BFGCCJNGFDK HitTimeSlowType { get; set; }
	public IKJCLBLHLBC HitTimeSlowIntensity { get; set; }
	public HitMotionParams HitMotion { get; set; }
	public float HitPosHeight { get; set; }
	public float HitAngleHorizontal { get; set; }
	public float HitAngleVertical { get; set; }
	public TargetEvaluator HitSource { get; set; }
	public bool TriggerHPBarAnimation { get; set; }
	public bool ScreenSpaceFloatMsg { get; set; }
}
