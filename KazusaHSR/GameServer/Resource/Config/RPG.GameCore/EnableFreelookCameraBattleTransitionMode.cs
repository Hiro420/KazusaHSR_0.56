namespace KazusaHSR.GameServer.Resource;

public class EnableFreelookCameraBattleTransitionMode : TaskConfig
{
	public bool IsBattleToMaze { get; set; }
	public bool Enable { get; set; }
	public float IgnoreAttackerDistance { get; set; }
}
