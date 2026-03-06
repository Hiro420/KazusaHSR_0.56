namespace KazusaHSR.GameServer.Resource;

public class SetBillboardInfo : TaskConfig
{
	public CharacterType TargetCharacterType { get; set; }
	public uint ID { get; set; }
	public uint GroupID { get; set; }
	public TextID Name { get; set; }
	public TextID Title { get; set; }
	public uint MapIconType { get; set; }
}
