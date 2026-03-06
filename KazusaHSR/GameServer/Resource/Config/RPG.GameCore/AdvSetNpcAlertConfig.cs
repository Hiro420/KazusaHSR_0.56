namespace KazusaHSR.GameServer.Resource;

public class AdvSetNpcAlertConfig : TaskConfig
{
	public float AlertLimitMin { get; set; }
	public float AlertLimitMax { get; set; }
	public float AlertGuardLimitMin { get; set; }
	public float AlertGuardLimitMax { get; set; }
	public float AlertDeclineSpeed { get; set; }
	public float AlertDeclineProtectTime { get; set; }
}
