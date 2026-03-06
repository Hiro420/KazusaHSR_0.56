namespace KazusaHSR.GameServer.Resource;

public class SetCharacterVisiable : TaskConfig
{
	public CharacterType CharType { get; set; }
	public string CharacterUniqueName { get; set; }
	public bool Visiable { get; set; }
	public bool MuteColliderWhenInvisiable { get; set; }
}
