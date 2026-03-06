namespace KazusaHSR.GameServer.Resource;

public class AnimTriggerRandomPlay : TaskConfig
{
	public float ActiveDelay { get; set; }
	public float ActiveDelayRange { get; set; }
	public AnimWeighted[] AnimList { get; set; }
}
