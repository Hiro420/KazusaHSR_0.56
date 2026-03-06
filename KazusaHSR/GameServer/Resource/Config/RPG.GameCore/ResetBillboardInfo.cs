namespace KazusaHSR.GameServer.Resource;

public class ResetBillboardInfo : TaskConfig
{
	public CharacterType TargetCharacterType { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
}
