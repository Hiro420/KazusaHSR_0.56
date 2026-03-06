namespace KazusaHSR.GameServer.Resource;

public class SetMainLightDir : TaskConfig
{
	public bool IsSyncSceneLight { get; set; }
	public bool IsSetActive { get; set; }
	public MVector3 Rotation { get; set; }
	public bool UseCharacterShadowFactor { get; set; }
	public float CharacterShadowFactor { get; set; }
}
