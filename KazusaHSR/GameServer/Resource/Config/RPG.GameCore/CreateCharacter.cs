namespace KazusaHSR.GameServer.Resource;

public class CreateCharacter : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public NDCCEODGDCK Catgory { get; set; }
	public string AvatarID { get; set; }
	public string AreaName { get; set; }
	public string AnchorName { get; set; }
}
