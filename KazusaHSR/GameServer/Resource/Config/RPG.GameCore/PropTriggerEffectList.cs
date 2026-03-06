namespace KazusaHSR.GameServer.Resource;

public class PropTriggerEffectList : TaskConfig
{
	public string UniqueName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public EffectConfig[] EffectList { get; set; }
}
