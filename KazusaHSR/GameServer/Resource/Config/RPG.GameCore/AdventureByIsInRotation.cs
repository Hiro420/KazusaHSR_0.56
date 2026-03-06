namespace KazusaHSR.GameServer.Resource;

public class AdventureByIsInRotation : PredicateConfig
{
	public uint TargetGroupID { get; set; }
	public uint TargetGroupNPCID { get; set; }
	public string TargetName { get; set; }
	public CharacterType TargetType { get; set; }
	public uint NpcID { get; set; }
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
	public MVector3 Rotation { get; set; }
	public float AngleTolerate { get; set; }
}
