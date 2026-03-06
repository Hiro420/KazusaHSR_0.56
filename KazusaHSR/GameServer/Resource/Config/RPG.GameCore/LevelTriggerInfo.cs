namespace KazusaHSR.GameServer.Resource;

public class LevelTriggerInfo
{
	public GOMGKEJFAKF Shape { get; set; }
	public float Radius { get; set; }
	public float DimX { get; set; }
	public float DimY { get; set; }
	public float DimZ { get; set; }
	public uint[] TargetCamps { get; set; }
	public int TriggerTimes { get; set; }
	public float CD { get; set; }
	public MVector3 Offset { get; set; }
	public bool Server { get; set; }
}
