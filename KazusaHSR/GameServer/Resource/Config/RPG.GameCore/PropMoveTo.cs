namespace KazusaHSR.GameServer.Resource;

public class PropMoveTo : TaskConfig
{
	public string UniqueName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public string RelativePath { get; set; }
	public float Duration { get; set; }
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
	public DynamicString LevelAreaKey { get; set; }
	public MVector3 TargetPosition { get; set; }
	public MVector3 TargetRotation { get; set; }
	public bool WaitFinish { get; set; }
}
