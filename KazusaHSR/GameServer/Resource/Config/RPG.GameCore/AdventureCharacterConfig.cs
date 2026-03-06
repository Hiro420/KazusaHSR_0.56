namespace KazusaHSR.GameServer.Resource;

public class AdventureCharacterConfig
{
	public float WalkSpeedRatio { get; set; }
	public float RunSpeedRatio { get; set; }
	public float NavTurnSpeed { get; set; }
	public float NavTurnBackTurnSpeed { get; set; }
	public LJFNHDGJPMC ShoesType { get; set; }
	public float LockTargetDistance { get; set; }
	public float MazeSkillDistance { get; set; }
	public string[] AnimEventConfigList { get; set; }
	public string CommonAnimZoneConfigPath { get; set; }
	public string AnimZoneConfigPath { get; set; }
	public string LookAtPhoneStartConfigPath { get; set; }
	public string LookAtPhoneEndConfigPath { get; set; }
	public string PhonePrefabPath { get; set; }
	public string DefaultAnimatorPath { get; set; }
	public string PerformanceAnimatorPath { get; set; }
	public AdventureSkillConfig[] SkillList { get; set; }
	public SkillAbilityConfig[] SkillAbilityList { get; set; }
	public string[] AbilityList { get; set; }
	public string[] SoundBankList { get; set; }
	// public OKCPGJLFDDK DynamicValues { get; set; }
	public bool BeHitRotate { get; set; }
	public bool EnableIK { get; set; }
	public float ModelScale { get; set; }
	public float ColliderScale { get; set; }
}
