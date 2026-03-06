namespace KazusaHSR.GameServer.Resource;

public class AdvNpcFaceToPlayer : TaskConfig
{
	public bool FromDialog { get; set; }
	public uint GroupID { get; set; }
	public uint GroupNpcID { get; set; }
	public uint PlayerInGroupID { get; set; }
	public uint PlayerInGroupNpcID { get; set; }
	public float Duration { get; set; }
	public bool TryFaceToFace { get; set; }
	public bool TurnBackOnGraphEnd { get; set; }
}
