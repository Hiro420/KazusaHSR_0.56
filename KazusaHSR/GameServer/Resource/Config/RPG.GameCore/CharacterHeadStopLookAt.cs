namespace KazusaHSR.GameServer.Resource;

public class CharacterHeadStopLookAt : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public float FadeOutTime { get; set; }
	public bool WaitUntilFinish { get; set; }
}
