namespace KazusaHSR.GameServer.Resource;

public class PropMove : TaskConfig
{
	public string UniqueName { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public string RelativePath { get; set; }
	public float Duration { get; set; }
	public MVector3 DeltaPosition { get; set; }
	public MVector3 DeltaRotation { get; set; }
}
