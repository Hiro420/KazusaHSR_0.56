namespace KazusaHSR.GameServer.Resource;

public class TriggerLoopScene : TaskConfig
{
	public EAIAPLLKLEA LoopMode { get; set; }
	public MVector3 PositionOffset { get; set; }
	public string ModelPath { get; set; }
	public MVector3 CameraPositionOffset { get; set; }
	public MVector3 CameraDirection { get; set; }
	public MVector3 CameraRotationOffset { get; set; }
	public float MoveSpeed { get; set; }
	public uint SceneSize { get; set; }
	public float SceneLength { get; set; }
	public float MaxRollbackLength { get; set; }
	public LoopMoveObject[] MoveObjects { get; set; }
}
