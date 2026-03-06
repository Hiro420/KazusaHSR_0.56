namespace KazusaHSR.GameServer.Resource;

public class EntityVisiableInfo
{
	public CharacterType CharType { get; set; }
	public uint GroupID { get; set; }
	public uint GroupNPCID { get; set; }
	public bool RenderVisible { get; set; }
	public bool MuteColliderWhenInvisiable { get; set; }
}
