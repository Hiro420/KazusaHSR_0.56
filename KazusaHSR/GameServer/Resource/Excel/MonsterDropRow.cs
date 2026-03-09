namespace KazusaHSR.GameServer.Resource.Excel;

public class MonsterDropRow
{
	public uint MonsterTemplateID { get; set; }
	public uint WorldLevel { get; set; }
	public uint AvatarExpReward { get; set; }
	public uint[] DisplayDropItemList { get; set; }
}
