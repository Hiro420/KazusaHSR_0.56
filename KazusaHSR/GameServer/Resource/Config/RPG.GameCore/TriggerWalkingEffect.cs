namespace KazusaHSR.GameServer.Resource;

public class TriggerWalkingEffect : TaskConfig
{
	public string FootName { get; set; }
	public MVector3 PositionOffset { get; set; }
	public MVector3 RotationOffset { get; set; }
	public MVector3 ScaleOffset { get; set; }
	public string EffectPrefab { get; set; }
	public Dictionary<string, string> OverridePrefabMap { get; set; }
}
