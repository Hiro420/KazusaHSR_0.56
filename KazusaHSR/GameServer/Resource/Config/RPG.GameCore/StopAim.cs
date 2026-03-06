namespace KazusaHSR.GameServer.Resource;

public class StopAim : TaskConfig
{
	public string AimerName { get; set; }
	public float TransitTime { get; set; }
	public bool IsEffect { get; set; }
	public string EffectPath { get; set; }
}
