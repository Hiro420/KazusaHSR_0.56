namespace KazusaHSR.GameServer.Resource;

public class RandomConfig : CompositeConfig
{
	public FixPoint[] OddsList { get; set; }
	public TaskConfig[] TaskList { get; set; }
}
