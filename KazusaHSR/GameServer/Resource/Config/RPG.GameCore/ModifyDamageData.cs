namespace KazusaHSR.GameServer.Resource;

public class ModifyDamageData : TaskConfig
{
	public DynamicFloat AttackData_DamageValue { get; set; }
	public DynamicFloat AttackData_DamagePercentage { get; set; }
	public DynamicFloat AttackData_SPHitRatio { get; set; }
	public DynamicFloat Attacker_AttackAddedRatio { get; set; }
	public DynamicFloat Attacker_AttackDelta { get; set; }
	public DynamicFloat Attacker_PhysicalAddedRatio { get; set; }
	public DynamicFloat Attacker_FireAddedRatio { get; set; }
	public DynamicFloat Attacker_IceAddedRatio { get; set; }
	public DynamicFloat Attacker_ThunderAddedRatio { get; set; }
	public DynamicFloat Attacker_WindAddedRatio { get; set; }
	public DynamicFloat Attacker_QuantumAddedRatio { get; set; }
	public DynamicFloat Attacker_ImaginaryAddedRatio { get; set; }
	public DynamicFloat Attacker_AllDamageAddedRatio { get; set; }
	public DynamicFloat Attacker_AllDamageTypeAddedRatio { get; set; }
	public DynamicFloat Attacker_PhysicalPenetrate { get; set; }
	public DynamicFloat Attacker_FirePenetrate { get; set; }
	public DynamicFloat Attacker_IcePenetrate { get; set; }
	public DynamicFloat Attacker_ThunderPenetrate { get; set; }
	public DynamicFloat Attacker_WindPenetrate { get; set; }
	public DynamicFloat Attacker_QuantumPenetrate { get; set; }
	public DynamicFloat Attacker_ImaginaryPenetrate { get; set; }
	public DynamicFloat Attacker_FatigueRatio { get; set; }
	public DynamicFloat Attacker_CriticalChance { get; set; }
	public DynamicFloat Attacker_CriticalDamage { get; set; }
	public DynamicFloat Attacker_SPRatio { get; set; }
	public DynamicFloat AttackerSkill_SPBase { get; set; }
	public DynamicFloat Defender_DefenceAddedRatio { get; set; }
	public DynamicFloat Defender_DefenceDelta { get; set; }
	public DynamicFloat Defender_PhysicalResistance { get; set; }
	public DynamicFloat Defender_FireResistance { get; set; }
	public DynamicFloat Defender_IceResistance { get; set; }
	public DynamicFloat Defender_ThunderResistance { get; set; }
	public DynamicFloat Defender_WindResistance { get; set; }
	public DynamicFloat Defender_QuantumResistance { get; set; }
	public DynamicFloat Defender_ImaginaryResistance { get; set; }
	public DynamicFloat Defender_AllDamageTypeResistance { get; set; }
	public DynamicFloat Defender_AllDamageReduce { get; set; }
	public DynamicFloat Defender_PhysicalTakenRatio { get; set; }
	public DynamicFloat Defender_FireTakenRatio { get; set; }
	public DynamicFloat Defender_IceTakenRatio { get; set; }
	public DynamicFloat Defender_ThunderTakenRatio { get; set; }
	public DynamicFloat Defender_WindTakenRatio { get; set; }
	public DynamicFloat Defender_QuantumTakenRatio { get; set; }
	public DynamicFloat Defender_ImaginaryTakenRatio { get; set; }
	public DynamicFloat Defender_AllDamageTypeTakenRatio { get; set; }
}
