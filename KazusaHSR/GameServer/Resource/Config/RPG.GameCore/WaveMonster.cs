namespace KazusaHSR.GameServer.Resource;

public class WaveMonster : TaskConfig
{
	public SingleMonsterInfo[] MonsterList { get; set; }
	public int LevelWaveIndex { get; set; }
	public string InitAnimStateName { get; set; }
	public bool WaitDie { get; set; }
	public string TriggerCustomStringOnCreated { get; set; }
	public bool Hide { get; set; }
}
