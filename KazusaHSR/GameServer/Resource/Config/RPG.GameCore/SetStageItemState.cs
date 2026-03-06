namespace KazusaHSR.GameServer.Resource;

public class SetStageItemState : TaskConfig
{
	public StageItemAlias[] ItemList { get; set; }
	public bool EnableState { get; set; }
}
