namespace KazusaHSR.GameServer.Resource;

public class AIMonsterIDSelector : AISelector
{
	public uint[] MonsterIDList { get; set; }
	public bool InverseResultFlag { get; set; }
}
