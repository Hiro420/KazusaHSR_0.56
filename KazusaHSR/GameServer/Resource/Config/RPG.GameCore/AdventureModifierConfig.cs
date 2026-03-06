namespace KazusaHSR.GameServer.Resource;

public class AdventureModifierConfig : ModifierConfig
{
	public float LifeTime { get; set; }
	public int Level { get; set; }
	public int LevelMax { get; set; }
	public bool IsCountDownAfterBattle { get; set; }
	public DHODCAEFJMC[] BehaviorFlagList { get; set; }
	public float TickInterval { get; set; }
	public TaskConfig[] OnInterval { get; set; }
	public TaskConfig[] OnCreate { get; set; }
	public TaskConfig[] OnDestroy { get; set; }
	public TaskConfig[] OnStack { get; set; }
	public TaskConfig[] OnAttack { get; set; }
	public TaskConfig[] OnBeforeBattle { get; set; }
	public TaskConfig[] OnAfterBattle { get; set; }
	public TaskConfig[] OnStage { get; set; }
	public TaskConfig[] OnUnstage { get; set; }
	public BNNBONMACPF MazeBuffType { get; set; }
}
