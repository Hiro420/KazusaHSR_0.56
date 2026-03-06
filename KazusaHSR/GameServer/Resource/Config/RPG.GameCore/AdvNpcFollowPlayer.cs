namespace KazusaHSR.GameServer.Resource;

public class AdvNpcFollowPlayer : TaskConfig
{
	public CharacterMotionFlag MotionFlag { get; set; }
	public float KeepDistance { get; set; }
	public float FollowDistance { get; set; }
	public float WaitProtectTime { get; set; }
	public float WaitActionTime { get; set; }
	public TaskConfig WaitActionTask { get; set; }
	public float TransferDistance { get; set; }
}
