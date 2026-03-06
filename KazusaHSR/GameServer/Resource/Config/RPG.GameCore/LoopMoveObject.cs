namespace KazusaHSR.GameServer.Resource;

public class LoopMoveObject
{
	public string UniqueName { get; set; }
	public string AvatarID { get; set; }
	public float MoveSpeed { get; set; }
	public float MoveDelay { get; set; }
	public MVector3 MoveDirection { get; set; }
	public MVector3 PositionOffset { get; set; }
	public MVector3 RotationOffset { get; set; }
}
