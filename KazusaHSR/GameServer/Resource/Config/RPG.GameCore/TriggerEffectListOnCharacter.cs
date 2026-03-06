namespace KazusaHSR.GameServer.Resource;

public class TriggerEffectListOnCharacter : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public EffectConfig[] EffectList { get; set; }
}
