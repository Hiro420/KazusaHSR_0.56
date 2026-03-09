namespace KazusaHSR.GameServer.Resource;

public class LevelGraphConfig
{
	public LevelTaskSequence[] OnInitSequece { get; set; } = System.Array.Empty<LevelTaskSequence>();
	public LevelTaskSequence[] OnStartSequece { get; set; } = System.Array.Empty<LevelTaskSequence>();
}

public class LevelTaskSequence
{
	public TaskConfig[] TaskList { get; set; } = System.Array.Empty<TaskConfig>();
}
