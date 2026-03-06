namespace KazusaHSR.GameServer.Resource;

public class WaveMonsterMaxCount : TaskConfig
{
	public int Count { get; set; }
	public bool ReadFromTable { get; set; }
	public string DynamicKey { get; set; }
}
