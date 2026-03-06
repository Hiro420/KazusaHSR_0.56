namespace KazusaHSR.GameServer.Resource;

public class ReplaceCharacterMaterial : TaskConfig
{
	public bool IsReset { get; set; }
	public ReplaceMaterialConfig[] ReplaceConfig { get; set; }
}
