namespace KazusaHSR.GameServer.Resource;

public class ConstructBodyPart : TaskConfig
{
	public string PartName { get; set; }
	public DynamicFloat MonsterID { get; set; }
	public int TeamLocationOffset { get; set; }
	public int IdentifyType { get; set; }
	public bool LinkedStance { get; set; }
}
