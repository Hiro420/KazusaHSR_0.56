namespace KazusaHSR.GameServer.Resource;

public class ChangeMaterialGroup : TaskConfig
{
	public string CharacterUniqueName { get; set; }
	public string MaterialGroupName { get; set; }
	public bool ChangeToOriginal { get; set; }
}
